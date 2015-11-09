using System.Collections;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace DemoProject.Test
{
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class BusinessIdentifierSpecificationUnitTests
	{
		#region Fields

		/// <summary>
		/// 
		/// </summary>
		private IInCorrectBusinessIdMessageHandler _inCorrectBusinessIdMessageHandler;

		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		private ISpecification<string> Specification { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private IInCorrectBusinessIdMessageHandler InCorrectBusinessIdMessageHandler
		{
			get { return this._inCorrectBusinessIdMessageHandler ?? (this._inCorrectBusinessIdMessageHandler = CreateIInCorrectBusinessIdMessageHandler()); }
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable CasesIsSatisfiedByFalseNullOrWhiteSpace
		{
			get
			{
				Assert.IsNotNull(this.InCorrectBusinessIdMessageHandler);
				yield return
					new TestCaseData(null, InCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.NullOrWhiteSpace));
				yield return
					new TestCaseData(string.Empty,
						InCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.NullOrWhiteSpace));
				yield return
					new TestCaseData(" ", InCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.NullOrWhiteSpace));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable CasesIsSatisfiedByFalseInvalidLength
		{
			get
			{
				Assert.IsNotNull(this.InCorrectBusinessIdMessageHandler);
				var errorMessage = InCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.InvalidLength);
				yield return new TestCaseData("1", errorMessage);
				yield return new TestCaseData("12", errorMessage);
				yield return new TestCaseData("123", errorMessage);
				yield return new TestCaseData("1234", errorMessage);
				yield return new TestCaseData("12345", errorMessage);
				yield return new TestCaseData("123456", errorMessage);
				yield return new TestCaseData("1234567", errorMessage);
				yield return new TestCaseData("12345678", errorMessage);
				yield return new TestCaseData("1234567891", errorMessage);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable CasesIsSatisfiedByFalseIncorrectFormat
		{
			get
			{
				Assert.IsNotNull(this.InCorrectBusinessIdMessageHandler);
				var errorMessage = InCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.IncorrectFormat);
				yield return new TestCaseData("1234567a8", errorMessage);
				yield return new TestCaseData("1234567 8", errorMessage);
				yield return new TestCaseData("1234567-a", errorMessage);
				yield return new TestCaseData("abcdefg-a", errorMessage);
				yield return new TestCaseData("abcdefg-1", errorMessage);
				yield return new TestCaseData("-234567-8", errorMessage);
				yield return new TestCaseData("+234567-8", errorMessage);
				yield return new TestCaseData("0000000-0", errorMessage);
				yield return new TestCaseData("0616863-9", errorMessage);
				yield return new TestCaseData("0665075-0", errorMessage);
				yield return new TestCaseData("6577105-7", errorMessage);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable CasesIsSatisfiedByFalseVerificationNumberMustBeZero
		{
			get
			{
				Assert.IsNotNull(this.InCorrectBusinessIdMessageHandler);
				var errorMessage =
					InCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.VerificationNumberMustBeZero);
				yield return new TestCaseData("7075902-8", errorMessage);
				yield return new TestCaseData("8551720-8", errorMessage);
				yield return new TestCaseData("1843835-1", errorMessage);
				yield return new TestCaseData("3760777-4", errorMessage);
				yield return new TestCaseData("4535061-2", errorMessage);
				yield return new TestCaseData("0005856-7", errorMessage);
				yield return new TestCaseData("0005856-7", errorMessage);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable CasesIsSatisfiedByFalseIncorrectChecksum
		{
			get
			{
				Assert.IsNotNull(this.InCorrectBusinessIdMessageHandler);
				var errorMessage = InCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.IncorrectChecksum);
				yield return new TestCaseData("0007604-3", errorMessage);
				yield return new TestCaseData("0470688-9", errorMessage);				
				yield return new TestCaseData("1565323-2", errorMessage);
				yield return new TestCaseData("1655288-5", errorMessage);
				yield return new TestCaseData("1809470-2", errorMessage);
				yield return new TestCaseData("1901843-6", errorMessage);
				yield return new TestCaseData("2110128-9", errorMessage);
				yield return new TestCaseData("2236476-7", errorMessage);
				yield return new TestCaseData("2273404-9", errorMessage);
				yield return new TestCaseData("2464205-0", errorMessage);
				yield return new TestCaseData("2521711-3", errorMessage);
				yield return new TestCaseData("2670408-0", errorMessage);
				yield return new TestCaseData("3156547-5", errorMessage);
				yield return new TestCaseData("3185874-1", errorMessage);
				yield return new TestCaseData("3382360-8", errorMessage);				
				yield return new TestCaseData("5743128-6", errorMessage);
				yield return new TestCaseData("6443037-0", errorMessage);
				yield return new TestCaseData("6626155-8", errorMessage);
				yield return new TestCaseData("8141036-2", errorMessage);
				yield return new TestCaseData("8516648-6", errorMessage);
				yield return new TestCaseData("7665268-4", errorMessage);
				yield return new TestCaseData("8936511-5", errorMessage);				
				yield return new TestCaseData("8231302-7", errorMessage);				
				yield return new TestCaseData("7506724-8", errorMessage);
				yield return new TestCaseData("8108720-0", errorMessage);
				yield return new TestCaseData("7000005-6", errorMessage);				
				yield return new TestCaseData("6832146-3", errorMessage);
				yield return new TestCaseData("6634278-2", errorMessage);
				yield return new TestCaseData("8720788-6", errorMessage);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private IEnumerable CasesIsSatisfiedByTrue
		{
			get
			{
				Assert.IsNotNull(InCorrectBusinessIdMessageHandler);
				yield return new TestCaseData("0615863-4");
				yield return new TestCaseData("1565323-1");				
				yield return new TestCaseData("2670008-0");
				yield return new TestCaseData("8586648-6");
				yield return new TestCaseData("0075856-7");
				yield return new TestCaseData("3816373-7");
				yield return new TestCaseData("7666268-4");
				yield return new TestCaseData("5442765-9");
				yield return new TestCaseData("2110128-1");
				yield return new TestCaseData("6625155-8");
				yield return new TestCaseData("8136511-5");
				yield return new TestCaseData("3185877-1");
			}
		}
		#endregion

		#region  Methods

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private static IInCorrectBusinessIdMessageHandler CreateIInCorrectBusinessIdMessageHandler()
		{
			var inCorrectBusinessIdMessageHandler = MockRepository.GenerateMock<IInCorrectBusinessIdMessageHandler>();
			inCorrectBusinessIdMessageHandler.Stub(f => f.GetErrorMessage(Arg.Is(InCorrectBusinessIdCase.NullOrWhiteSpace))).Return("cannot be null");
			inCorrectBusinessIdMessageHandler.Stub(f => f.GetErrorMessage(Arg.Is(InCorrectBusinessIdCase.NullOrWhiteSpace))).Return("cannot be empty");
			inCorrectBusinessIdMessageHandler.Stub(f => f.GetErrorMessage(Arg.Is(InCorrectBusinessIdCase.InvalidLength))).Return("invalid length");
			inCorrectBusinessIdMessageHandler.Stub(f => f.GetErrorMessage(Arg.Is(InCorrectBusinessIdCase.IncorrectFormat))).Return("incorrect format");
			inCorrectBusinessIdMessageHandler.Stub(f => f.GetErrorMessage(Arg.Is(InCorrectBusinessIdCase.VerificationNumberMustBeZero))).Return("Verification number must be zero");
			inCorrectBusinessIdMessageHandler.Stub(f => f.GetErrorMessage(Arg.Is(InCorrectBusinessIdCase.IncorrectChecksum))).Return("Incorrect checksum");
			return inCorrectBusinessIdMessageHandler;
		}

		/// <summary>
		/// 
		/// </summary>			
		[SetUp]
		public void Setup()
		{
			Assert.IsNotNull(this.InCorrectBusinessIdMessageHandler);
			this.Specification = new BusinessIdentifierSpecification(this.InCorrectBusinessIdMessageHandler);
			Assert.IsNotNull(this.Specification);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="reason"></param>
		[Test]
		[TestCaseSource("CasesIsSatisfiedByFalseNullOrWhiteSpace")]
		[Category("Not satisfied items")]
		public void IsSatisfiedBy_False_NullOrWhiteSpace(string item, string reason)
		{
			BusinessIdentifierSpecificationAssert.IsSatisfiedByFalse(this.Specification, item, reason);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="reason"></param>
		[Test]
		[TestCaseSource("CasesIsSatisfiedByFalseInvalidLength")]
		[Category("Not satisfied items")]
		public void IsSatisfiedBy_False_InvalidLength(string item, string reason)
		{
			BusinessIdentifierSpecificationAssert.IsSatisfiedByFalse(this.Specification, item, reason);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="reason"></param>
		[Test]
		[TestCaseSource("CasesIsSatisfiedByFalseIncorrectFormat")]
		[Category("Not satisfied items")]
		public void IsSatisfiedBy_False_IncorrectFormat(string item, string reason)
		{
			BusinessIdentifierSpecificationAssert.IsSatisfiedByFalse(this.Specification, item, reason);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="reason"></param>
		[Test]
		[TestCaseSource("CasesIsSatisfiedByFalseVerificationNumberMustBeZero")]
		[Category("Not satisfied items")]
		public void IsSatisfiedBy_False_VerificationNumberMustBeZero(string item, string reason)
		{
			BusinessIdentifierSpecificationAssert.IsSatisfiedByFalse(this.Specification, item, reason);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="reason"></param>
		[Test]
		[TestCaseSource("CasesIsSatisfiedByFalseIncorrectChecksum")]
		[Category("Not satisfied items")]
		public void IsSatisfiedBy_False_IncorrectChecksum(string item, string reason)
		{
			BusinessIdentifierSpecificationAssert.IsSatisfiedByFalse(this.Specification, item, reason);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		[Test]
		[TestCaseSource("CasesIsSatisfiedByTrue")]
		[Category("Satisfied items")]
		public void IsSatisfiedBy_True(string item)
		{
			Assert.IsTrue(this.Specification.IsSatisfiedBy(item));
			Assert.AreEqual(this.Specification.ReasonsForDissatisfaction.Count(), 0);
		}
		#endregion
	}
}
