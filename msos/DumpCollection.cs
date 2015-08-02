﻿using CommandLine;
using Microsoft.Diagnostics.RuntimeExt;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msos
{
    [SupportedTargets(TargetType.DumpFile, TargetType.LiveProcess)]
    [Verb("!DumpCollection", HelpText = "Displays the contents of a collection. Arrays, lists, and dictionaries are supported at present.")]
    class DumpCollection : ICommand
    {
        [Value(0, Required = true)]
        public string ObjectAddress { get; set; }

        public void Execute(CommandExecutionContext context)
        {
            ulong objPtr;
            if (!CommandHelpers.ParseAndVerifyValidObjectAddress(context, ObjectAddress, out objPtr))
                return;

            var type = context.Heap.GetObjectType(objPtr);
            var dynamicObj = context.Heap.GetDynamicObject(objPtr);
            context.WriteLine("Type:   {0}", type.Name);
            if (type.IsArray || dynamicObj.IsList())
            {
                int length = dynamicObj.GetLength();
                context.WriteLine("Length: {0}", length);
                for (int i = 0; i < length; ++i)
                {
                    context.WriteLine("[{0}] {1}", i, dynamicObj[i]);
                }
            }
            else if (dynamicObj.IsDictionary())
            {
                context.WriteLine("Size:   {0}", dynamicObj.GetLength());
                context.WriteLine("{0,-40} {1,-40}", "Key", "Value");
                foreach (var kvp in dynamicObj.GetDictionaryItems())
                {
                    context.WriteLine("{0,-40} {1,-40}", kvp.Item1, kvp.Item2);
                }
            }
            else
            {
                context.WriteError("The specified object is not an array, list, or dictionary. " +
                    "These are the only collection types that are currently supported. For other " +
                    "collections, try using the !hq command to inspect the object's contents.");
                return;
            }
        }
    }
}
