﻿namespace FastFood.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Models;
    using FastFood.Models.Enums;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Orders;

    public class OrdersController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public OrdersController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var viewOrder = new CreateOrderViewModel
            {
                Items = this.context.Items.Select(x => x.Id).ToList(),
                Employees = this.context.Employees.Select(x => x.Id).ToList(),
            };

            return this.View(viewOrder);
        }

        [HttpPost]
        public IActionResult Create(CreateOrderInputModel model)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }
            var order = this.mapper.Map<Order>(model);
            order.DateTime = DateTime.Parse(DateTime.Now.ToString("G"));
            order.Type = Enum.Parse<OrderType>(model.Type);
            order.OrderItems.Add(new OrderItem()
            {
                ItemId = model.ItemId,
                Order = order
            });
            this.context.Orders.Add(order);
            this.context.SaveChanges();
          

            return this.RedirectToAction("All", "Orders");
        }

        public IActionResult All()
        {
            var order = this.context
                .Orders
                .ProjectTo<OrderAllViewModel>(mapper.ConfigurationProvider)
                .ToList();
            return this.View(order);
        }
    }
}
