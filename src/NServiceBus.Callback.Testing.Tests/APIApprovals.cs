﻿#if NET452
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ApprovalTests;
using NServiceBus.Callbacks.Testing;
using NUnit.Framework;
using PublicApiGenerator;

[TestFixture]
public class APIApprovals
{
    [Test]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Approve()
    {
        var combine = Path.Combine(TestContext.CurrentContext.TestDirectory, Path.GetFileName(typeof(TestableCallbackAwareSession).Assembly.Location));
        var assembly = Assembly.LoadFile(combine);
        var publicApi = Filter(ApiGenerator.GeneratePublicApi(assembly));

        Approvals.Verify(publicApi);
    }

    string Filter(string text)
    {
        return string.Join(Environment.NewLine, text.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries)
            .Where(l => !string.IsNullOrWhiteSpace(l))
        );
    }
}
#endif