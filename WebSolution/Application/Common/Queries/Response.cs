using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Common.Queries
{
    public class Response<T>
    {
        public List<T> Data { get; set; }

        public Response()
        {
        }


    }
}
