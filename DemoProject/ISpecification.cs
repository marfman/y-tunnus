using System.Collections.Generic;

namespace DemoProject
{
	public interface ISpecification<in TEntity>
	{
		/// <summary>
		/// 
		/// </summary>
		IEnumerable<string> ReasonsForDissatisfaction { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool IsSatisfiedBy(TEntity entity);
	}
}
