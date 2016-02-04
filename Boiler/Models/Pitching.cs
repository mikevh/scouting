// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using ServiceStack.OrmLite;

namespace Boiler.Models
{
	[Alias("Pitching")]
    public partial class Pitching : IHasId<int>, IHasAudit
    {
        [Alias("PitchingId")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int PlayerId { get; set; }
        public decimal? Mechanics { get; set; }
        public decimal? Velocity { get; set; }
        public decimal? Command { get; set; }
        public string PitchingNote { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
	}
}
#pragma warning restore 1591
