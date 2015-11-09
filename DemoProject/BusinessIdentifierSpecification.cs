using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DemoProject
{
	/// <summary>
	/// 
	/// </summary>
	public class BusinessIdentifierSpecification : ISpecification<string>
	{
		#region Fields

		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		private const int EntityLength = 9;

		/// <summary>
		/// 
		/// </summary>
		private const string EntityPattern = @"^([0-9]{7})-([0-9]{1})$";

		/// <summary>
		/// 
		/// </summary>
		private const int EntityDivider = 11;

		/// <summary>
		/// 
		/// </summary>
		private const int CounterMax = 10;

		/// <summary>
		/// 
		/// </summary>
		private readonly List<string> _reasonsForDissatisfaction = new List<string>();

		/// <summary>
		/// 
		/// </summary>
		private readonly IInCorrectBusinessIdMessageHandler _inCorrectBusinessIdMessageHandler;

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<string> ReasonsForDissatisfaction
		{
			get { return this._reasonsForDissatisfaction; }
		}

		#endregion

		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inCorrectBusinessIdMessageHandler"></param>
		public BusinessIdentifierSpecification(IInCorrectBusinessIdMessageHandler inCorrectBusinessIdMessageHandler)
		{
			if (inCorrectBusinessIdMessageHandler == null)
			{
				throw new ArgumentNullException("inCorrectBusinessIdMessageHandler");
			}
			this._inCorrectBusinessIdMessageHandler = inCorrectBusinessIdMessageHandler;
		}
		#endregion

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reason"></param>
		private void Set(string reason)
		{
			this._reasonsForDissatisfaction.Add(reason);
		}

		/// <summary>
		/// 
		/// </summary>
		private void Clear()
		{
			this._reasonsForDissatisfaction.Clear();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skipIndex"></param>
		/// <returns></returns>
		private static decimal? GetTotalCount(string entity, int? skipIndex = null)
		{
			var elements = new Dictionary<int, int>
			{
				{6, 2},
				{5, 4},
				{4, 8},
				{3, 5},
				{2, 10},
				{1, 9},
				{0, 7}
			};
			if (skipIndex.HasValue)
			{
				if (elements.ContainsKey(skipIndex.Value))
				{
					elements.Remove(skipIndex.Value);
				}
				else
				{
					return null;
				}
			}
			decimal totalCount = 0;
			foreach (var element in elements)
			{
				totalCount += (int.Parse(entity.Substring(element.Key, 1)) * element.Value);
			}
			return totalCount;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skipIndex"></param>
		/// <param name="counter"></param>
		/// <returns></returns>
		private bool HandleCorrectFormatEntity(string entity, int? skipIndex = null, int counter = 0)
		{
			counter++;
			if (counter >= CounterMax)
			{
				this.Set(this._inCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.IncorrectFormat));
				return false;
			}

			//http://www.finlex.fi/fi/laki/ajantasa/2001/20010288

			//Yritys- ja yhteisötunnuksen tekninen muoto
			//Yritys- ja yhteisötunnuksen muodostavat järjestysnumero ja tarkistusnumero mainitussa järjestyksessä. Järjestysnumero on enintään seitsemännumeroinen.
			//Tarkistusnumeron laskemiseksi kerrotaan oikealta vasemmalle lukien järjestysnumeron
			//ensimmäinen numero luvulla	2,
			//toinen " "	4,
			//kolmas " "	8,
			//neljäs " "	5,
			//viides " "	10,
			//kuudes " "	9,
			//seitsemäs " "	7
			//sekä lasketaan näin saadut tulot yhteen ja jaetaan summa luvulla 11. Tarkistusnumero määräytyy jakolaskun jakojäännöksen perusteella siten, että jos jakojäännös on nolla, on tarkistusnumero nolla, jos jakojäännös on suurempi kuin yksi, on tarkistusnumero erotus, joka saadaan vähentämällä jakojäännös luvusta 11, ja jos jakojäännös on yksi, jätetään tarkistusnumeroa vastaava järjestysnumero käyttämättä

			var totalCount = GetTotalCount(entity, skipIndex);
			if (!totalCount.HasValue || totalCount.Value <= 0)
			{
				this.Set(this._inCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.IncorrectFormat));
				return false;
			}
			var divided = (totalCount.Value % EntityDivider);
			var checkDigit = int.Parse(entity.Substring(8, 1));
			if (divided == 0)
			{
				//Tarkistusnumero määräytyy jakolaskun jakojäännöksen perusteella siten, että jos jakojäännös on nolla, on tarkistusnumero nolla
				if (checkDigit == 0)
				{
					return true;
				}
				this.Set(this._inCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.VerificationNumberMustBeZero));
				return false;
			}
			if (divided > 1)
			{
				//jos jakojäännös on suurempi kuin yksi, on tarkistusnumero erotus, joka saadaan vähentämällä jakojäännös luvusta 11
				if ((EntityDivider - divided) == checkDigit)
				{
					return true;
				}
				this.Set(this._inCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.IncorrectChecksum));
				return false;
			}
			//ja jos jakojäännös on yksi, jätetään tarkistusnumeroa vastaava järjestysnumero käyttämättä
			return (divided == 1 && HandleCorrectFormatEntity(entity, checkDigit, counter));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool IsSatisfiedBy(string entity)
		{
			this.Clear();

			if (string.IsNullOrWhiteSpace(entity))
			{
				this.Set(this._inCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.NullOrWhiteSpace));
				return false;
			}
			if (entity.Length != EntityLength)
			{
				this.Set(this._inCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.InvalidLength));
				return false;
			}
			var regex = new Regex(EntityPattern);
			if (regex.IsMatch(entity))
			{
				return HandleCorrectFormatEntity(entity);
			}
			this.Set(this._inCorrectBusinessIdMessageHandler.GetErrorMessage(InCorrectBusinessIdCase.IncorrectFormat));
			return false;
		}
		#endregion
	}
}
