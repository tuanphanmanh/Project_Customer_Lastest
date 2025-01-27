﻿using System.ComponentModel.DataAnnotations;

namespace esign.Friendships.Dto.Ver1
{
    public class BlockUserInput 
    {
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        public int? TenantId { get; set; }
    }
}