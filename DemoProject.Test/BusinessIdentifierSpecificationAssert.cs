using System.Linq;
using NUnit.Framework;

namespace DemoProject.Test
{
	public static class BusinessIdentifierSpecificationAssert
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="specification"></param>
		/// <param name="item"></param>
		/// <param name="reason"></param>
		public static void IsSatisfiedByFalse(ISpecification<string> specification, string item, string reason)
		{
			Assert.IsNotNull(specification);
			Assert.IsFalse(specification.IsSatisfiedBy(item));
			Assert.AreEqual(specification.ReasonsForDissatisfaction.Count(), 1);
			Assert.AreEqual(specification.ReasonsForDissatisfaction.Single(), reason);
		}
	}
}
