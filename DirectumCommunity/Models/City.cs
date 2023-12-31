﻿namespace DirectumCommunity.Models;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }

    public void Update(City city)
    {
        Name = city.Name;
        Status = city.Status;
    }
}