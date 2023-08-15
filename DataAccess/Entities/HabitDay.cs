﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class HabitDay
    {
        public int Id { get; set; }

        public int HabitId { get; set; }

        public DateTime Date { get; set; }

        public bool IsCompleted { get; set; }
    }
}
