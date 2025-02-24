﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAMAUIGardenCentreApp.Models;

[Table("category")]
public class Category
{
    [PrimaryKey, AutoIncrement]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }


    [Column("description")]
    public string Description { get; set; }


    private string _imageUrl;

    [Column("image_url")]
    public string ImageUrl
    {
        get
        {
            return _imageUrl;
        }
        set
        {
            _imageUrl = value;
        }
    }
}
