﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAMAUIGardenCentreApp.Models;

namespace CAMAUIGardenCentreApp.Models;

public class CartItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; } = 1;
}
