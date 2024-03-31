using Application.Enums;
using Application.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application
{
    public class DishManager : IDishManager
    {
        private readonly ILogger _logger;
        private readonly Dictionary<TimeOfDay, Dictionary<DishType, string>> _dishes;
        public DishManager(ILogger logger)
        {
            _logger = logger;

            // Initialize dishes for morning and evening
            // This can be read from JSON file or from db
            _dishes = new Dictionary<TimeOfDay, Dictionary<DishType, string>>
            {
                [TimeOfDay.Morning] = new Dictionary<DishType, string>
                {
                    { DishType.Entrée, "egg" },
                    { DishType.Side, "toast" },
                    { DishType.Drink, "coffee" }
                },
                [TimeOfDay.Evening] = new Dictionary<DishType, string>
                {
                    { DishType.Entrée, "steak" },
                    { DishType.Side, "potato" },
                    { DishType.Drink, "wine" },
                    { DishType.Dessert, "cake" }
                }
            };
        }

        /// <summary>
        /// Takes an Order object, sorts the orders and builds a list of dishes to be returned. 
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<Dish> GetDishes(Order order, TimeOfDay timeOfDay)
        {
            if(!_dishes.ContainsKey(timeOfDay))
            {
                var errorMessage = "Invalid time of the day";
                //_logger.Error(errorMessage);
                throw new ApplicationException(errorMessage);
            }

            var returnValue = new List<Dish>();
            order.Dishes.Sort();
            foreach (var dishType in order.Dishes)
            {
                AddOrderToList((DishType)dishType, returnValue, timeOfDay);
            }
            return returnValue;
        }

        /// <summary>
        /// Takes an int, representing an order type, tries to find it in the list.
        /// If the dish type does not exist, add it and set count to 1
        /// If the type exists, check if multiples are allowed and increment that instances count by one
        /// else throw error
        /// </summary>
        /// <param name="order">int, represents a dishtype</param>
        /// <param name="returnValue">a list of dishes, - get appended to or changed </param>
        private void AddOrderToList(DishType order, List<Dish> returnValue, TimeOfDay timeOfDay)
        {
            if (!_dishes[timeOfDay].ContainsKey(order))
            {
                var errorMessage = "Invalid time of the day";
                //_logger.Error(errorMessage);
                throw new ApplicationException(errorMessage);
            }

            string orderName = _dishes[timeOfDay][order];
            var existingOrder = returnValue.SingleOrDefault(x => x.DishName == orderName);
            if (existingOrder == null)
            {
                returnValue.Add(new Dish
                {
                    DishName = orderName,
                    Count = 1
                });
            } else if (IsMultipleAllowed(order, timeOfDay))
            {
                existingOrder.Count++;
            }
            else
            {
                //_logger.Error("Multiple {0}(s) not allowed", orderName);
                throw new ApplicationException(string.Format("Multiple {0}(s) not allowed", orderName));
            }
        }

        private bool IsMultipleAllowed(DishType order, TimeOfDay timeOfDay)
        {
            if (timeOfDay == TimeOfDay.Morning && order == DishType.Drink)
            {
                return true; // Coffee can have multiple orders in the morning
            }
            else if (timeOfDay == TimeOfDay.Evening && order == DishType.Side)
            {
                return true; // Potato can have multiple orders in the evening
            }
            return false;
        }
    }
}