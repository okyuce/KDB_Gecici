﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Csb.YerindeDonusum.Application.Resolver;

public class IgnorePropertiesResolver : DefaultContractResolver
{
    private readonly HashSet<string> ignoreProps;
    public IgnorePropertiesResolver(IEnumerable<string> propNamesToIgnore)
    {
        this.ignoreProps = new HashSet<string>(propNamesToIgnore.Select(s => s.ToLower()));
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        if (this.ignoreProps.Contains(property.PropertyName.ToLower()))
        {
            property.ShouldSerialize = _ => false;
        }
        return property;
    }
}