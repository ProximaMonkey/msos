﻿using Microsoft.Diagnostics.Runtime;
using Microsoft.Diagnostics.RuntimeExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace msos
{
    static class ClrFieldExtensions
    {
        public static string GetFieldTypeNameTrimmed(this ClrField field, int trim = 16)
        {
            var fieldTypeName = field.Type == null ? field.ElementType.ToString() : field.Type.Name;
            if (fieldTypeName.Length > trim)
            {
                fieldTypeName = "..." + fieldTypeName.Substring(fieldTypeName.Length - trim);
            }
            return fieldTypeName;
        }

        public static bool IsObjectReferenceNotString(this ClrField field)
        {
            return (field.ElementType != ClrElementType.String) && field.IsOfObjectReferenceType();
        }
    }
}
