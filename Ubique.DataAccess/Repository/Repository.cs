﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ubique.DataAccess.Data;
using Ubique.DataAccess.Repository.IRepository;

namespace Ubique.DataAccess.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;

		public Repository(ApplicationDbContext db)
		{
			_db = db;
			dbSet = _db.Set<T>();
		}

		public void Add(T entity)
		{
			dbSet.Add(entity);
		}

		public T Get(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = dbSet;
			query = query.Where(filter);
			return query.FirstOrDefault();
		}

		public IEnumerable<T> GetList(Expression<Func<T, bool>> filter)
		{
			IQueryable<T> query = dbSet;
			query = query.Where(filter);
			return query.ToList();
		}

		public IEnumerable<T> GetAll()
		{
			IQueryable<T> query = dbSet;
			return query.ToList();
		}

		public void Remove(T entity)
		{
			dbSet.Remove(entity);
		}

		public void RemoveRange(IEnumerable<T> entities)
		{
			dbSet.RemoveRange(entities);
		}
	}
}
