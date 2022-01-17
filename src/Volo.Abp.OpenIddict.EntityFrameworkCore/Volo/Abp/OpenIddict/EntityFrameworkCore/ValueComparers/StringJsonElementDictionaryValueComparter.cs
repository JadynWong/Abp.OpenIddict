using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore.ValueConverters;

public class StringJsonElementDictionaryValueComparter : ValueComparer<Dictionary<string, JsonElement>>
{
    public StringJsonElementDictionaryValueComparter()
        : base(
              (d1, d2) => d1.SequenceEqual(d2),
              d => d.Aggregate(0, (k, v) => HashCode.Combine(k, v.GetHashCode())),
              d => new Dictionary<string, JsonElement>(d))
    {
    }
}