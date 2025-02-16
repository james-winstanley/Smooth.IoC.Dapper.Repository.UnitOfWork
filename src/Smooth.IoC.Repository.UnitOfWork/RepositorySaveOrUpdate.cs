﻿using Smooth.IoC.Repository.UnitOfWork.Extensions;
using Smooth.IoC.UnitOfWork.Interfaces;
using System;
using System.Threading.Tasks;

namespace Smooth.IoC.Repository.UnitOfWork;

public abstract partial class Repository<TEntity, TPk>
    where TEntity : class
    where TPk : IComparable
{
    public virtual TPk SaveOrUpdate(TEntity entity, IUnitOfWork uow)
    {
        if (TryAllKeysDefault(entity))
        {
            uow.Insert(entity);
        }
        else
        {
            uow.Update(entity);
        }

        TPk primaryKeyValue = GetPrimaryKeyValue(entity);
        return primaryKeyValue != null
            ? primaryKeyValue
            : default;
    }

    public virtual TPk SaveOrUpdate<TSession>(TEntity entity) 
        where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        TPk primaryKeyValue = SaveOrUpdate(entity, uow);
        uow.Commit();
        return primaryKeyValue;
    }

    public virtual async Task<TPk> SaveOrUpdateAsync(TEntity entity, IUnitOfWork uow)
    {
        return await Task.Run(() =>
        {
            if (TryAllKeysDefault(entity))
            {
                uow.InsertAsync(entity);
            }
            else
            {
                uow.UpdateAsync(entity);
            }

            TPk primaryKeyValue = GetPrimaryKeyValue(entity);
            return primaryKeyValue != null
                ? primaryKeyValue
                : default;
        });
    }

    public virtual async Task<TPk> SaveOrUpdateAsync<TSession>(TEntity entity) 
        where TSession : class, ISession
    {
        using IUnitOfWork uow = Factory.Create<IUnitOfWork, TSession>();
        TPk primaryKeyValue = await SaveOrUpdateAsync(entity, uow);
        uow.Commit();
        return primaryKeyValue;
    }
}