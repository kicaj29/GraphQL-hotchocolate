using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.Core
{
    /// <summary>
    /// A review of a particular movie.
    /// </summary>
    public class Review
    {
        public int BookId { get; set; }
        /// <summary>
        /// The number of stars given for this review.
        /// </summary>
        public int Stars { get; set; }

        /// <summary>
        /// An explanation for the rating.
        /// </summary>
        public string Commentary { get; set; }
    }
}
