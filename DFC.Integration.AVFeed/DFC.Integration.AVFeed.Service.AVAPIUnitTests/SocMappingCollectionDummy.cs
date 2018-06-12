namespace DFC.Integration.AVFeed.Service.AVAPIUnitTests
{
    using System;
    using System.Collections.Generic;
    using Data.Models;

    public class SocMappingCollectionDummy
    {
        public static IEnumerable<object[]> SocMappingCollections()
        {
            yield return new object[]
            {
                "1234", Guid.NewGuid(),new string[] { null }, new string[] { string.Empty }
            };
            yield return new object[]
            {
                "1234", Guid.NewGuid(),new string[] { null }, new string[] { "512", string.Empty }
            };
            yield return new object[]
            {
                "3211", Guid.NewGuid(),new string[] { "225",string.Empty,null },new string[] { "512" ,string.Empty,string.Empty}
            };
            yield return new object[]
            {
                "1111", Guid.NewGuid(), new string[] { null }, new string[] { string.Empty }
            };
            yield return new object[]
            {
                 "2222", Guid.NewGuid(), new string[] { null }, new string[] { "512" ,"612"}
            };

        }
    }
}