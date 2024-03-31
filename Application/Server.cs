using Application.Enums;
using Application.Models;
using Serilog;
using System;
using System.Collections.Generic;

namespace Application
{
    public class Server : IServer
    {
        private readonly ILogger _logger;
        private readonly IDishManager _dishManager;

        public Server(ILogger logger, IDishManager dishManager)
        {
            _logger = logger;
            _dishManager = dishManager;
        }
        
        public string TakeOrder(string unparsedOrder, TimeOfDay timeOfDay)
        {
            try
            {
                Order order = ParseOrder(unparsedOrder);
                List<Dish> dishes = _dishManager.GetDishes(order, timeOfDay);
                string returnValue = FormatOutput(dishes);
                return returnValue;
            }
            catch (ApplicationException ex)
            {
                //_logger.Error(ex, "Error processing order");
                return "error";
            }
        }


        private Order ParseOrder(string unparsedOrder)
        {
            var returnValue = new Order
            {
                Dishes = new List<int>()
            };

            var orderItems = unparsedOrder.Split(',');
            foreach (var orderItem in orderItems)
            {
                if (int.TryParse(orderItem, out int parsedOrder))
                {
                    returnValue.Dishes.Add(parsedOrder);
                }
                else
                {
                    throw new ApplicationException("Order needs to be comma separated list of numbers");
                }
            }
            return returnValue;
        }

        private string FormatOutput(List<Dish> dishes)
        {
            var returnValue = "";

            foreach (var dish in dishes)
            {
                returnValue = returnValue + string.Format(",{0}{1}", dish.DishName, GetMultiple(dish.Count));
            }

            if (returnValue.StartsWith(","))
            {
                returnValue = returnValue.TrimStart(',');
            }

            return returnValue;
        }

        private object GetMultiple(int count)
        {
            if (count > 1)
            {
                return string.Format("(x{0})", count);
            }
            return "";
        }
    }
}
