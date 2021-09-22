﻿using System;

namespace Shops.Services
{
    public class RegisteredProductInService
    {
        public RegisteredProductInService(int id, string name)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Id { get; }
        public string Name { get; }
    }
}