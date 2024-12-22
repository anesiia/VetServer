using Microsoft.EntityFrameworkCore;

namespace VetServer.Models
{
    [Keyless]
    public class AppointmentCount
    {
        public int Counte {  get; set; }
    }
}
