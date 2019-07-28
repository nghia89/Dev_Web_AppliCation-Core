using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppCore.Application.Dapper.ViewModels
{
    public class RevenueReportViewModel
    {
        public DateTime Date { get; set; }
        //Doanh thu
        public decimal Revenue { get; set; }
        //Lợi nhuận
        public decimal Profit { get; set; }
    }
}
