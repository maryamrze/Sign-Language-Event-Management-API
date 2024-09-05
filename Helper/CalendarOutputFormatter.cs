using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using a2.Dtos;
using a2.Models;

namespace a2.Helper{
    public class CalendarOuputFormatter : TextOutputFormatter{
        public CalendarOuputFormatter(){
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar; charset=utf-8"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            Event evt = (Event)context.Object;
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:jkau112");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"UID:{evt.Id}");
            sb.AppendLine($"DTSTAMP {DateTime.UtcNow:yyyyMMddTHHmmssZ}");
            sb.AppendLine($"DTSTART:{evt.Start}");
            sb.AppendLine($"DTEND:{evt.End}");
            sb.AppendLine($"SUMMARY:{evt.Summary}");
            sb.AppendLine($"DESCRIPTION:{evt.Description}");
            sb.AppendLine($"LOCATION:{evt.Location}");
            sb.AppendLine("END:VEVENT");
            sb.AppendLine("END:VCALENDAR");

            string outString = sb.ToString();
            byte[] outBytes = selectedEncoding.GetBytes(outString);
            var response = context.HttpContext.Response.Body;
            return response.WriteAsync(outBytes, 0, outBytes.Length);
        }
    }
}