using Application.Enums;

namespace Application
{
    public interface IServer
    {
        /// <summary>
        /// From an unparsed order, such as "1,2,3" returns a comma 
        /// separated list of dishes with their counts if count > 0
        /// </summary>
        /// <param name="unparsedOrder">for example 1,2,3</param>
        /// <returns>for example: steak,potato,wine</returns>
        string TakeOrder(string unparsedOrder, TimeOfDay timeOfDay);

    }
}