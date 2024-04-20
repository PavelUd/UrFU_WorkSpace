using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace UrFU_WorkSpace_API.Models;
[Table("workspace_reservations")]
public class Reservation
{
        [Key]
        [Column("reservation_id")]
        public int IdReservation { get; set; }
        
        [Column("object_id")]
        public int IdObject { get; set; }
        
        [Column("user_id")]
        public int IdUser { get; set; }
        
        [Column("workspace_id")]
        public int IdWorkspace { get; set; }
        
        [Column("time_start")]
        public TimeSpan TimeStart { get; set; }
        
        [Column("time_end")] 
        public TimeSpan TimeEnd { get; set; }
        
        [Column("date")]
        public DateOnly Date { get; set; }
        
}