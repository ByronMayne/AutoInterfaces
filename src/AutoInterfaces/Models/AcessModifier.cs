using System;
using System.Collections.Generic;
using System.Text;
using Vogen;

namespace AutoInterfaces.Models
{
    [ValueObject<string>(conversions: Conversions.None)]
    [Instance("Public", "public")]
    [Instance("Private", "private")]
    [Instance("Protected", "protected")]
    [Instance("PrivateProtected", "private protected")]
    [Instance("Internal", "internal")]
    internal partial struct MemberAcessModifier {}

    [ValueObject<string>(conversions: Conversions.None)]
    [Instance("Public", "public")]
    [Instance("Internal", "internal")]
    internal partial struct ClassAccessModifier { }
}
