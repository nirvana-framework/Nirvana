using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Nirvana.Util.Extensions
{
    public static class ReflectionExtensions
    {
        public static TypeBuilder GetTypeBuilder()
        {
            var an = new AssemblyName("DynamicAssembly" + new Random().Next(999999999));
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            var tb = moduleBuilder.DefineType("DynamicType"
                , TypeAttributes.Public |
                  TypeAttributes.Class |
                  TypeAttributes.AutoClass |
                  TypeAttributes.AnsiClass |
                  TypeAttributes.BeforeFieldInit |
                  TypeAttributes.AutoLayout
                , typeof(object));

            tb.DefineDefaultConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName);

            return tb;
        }

        public static void AddProperty(this TypeBuilder builder, string propertyName, Type propertyType)
        {
            var fieldBuilder = builder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            var propertyBuilder = builder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            var getPropertyBuiler = CreatePropertyGetter(builder, fieldBuilder);
            var setPropertyBuiler = CreatePropertySetter(builder, fieldBuilder);

            propertyBuilder.SetGetMethod(getPropertyBuiler);
            propertyBuilder.SetSetMethod(setPropertyBuiler);
        }

        private static MethodBuilder CreatePropertyGetter(this TypeBuilder builder, FieldBuilder fieldBuilder)
        {
            var getMethodBuilder =
                builder.DefineMethod("get_" + fieldBuilder.Name,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    fieldBuilder.FieldType, Type.EmptyTypes);

            var getIL = getMethodBuilder.GetILGenerator();

            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);

            return getMethodBuilder;
        }

        private static MethodBuilder CreatePropertySetter(this TypeBuilder builder, FieldBuilder fieldBuilder)
        {
            var setMethodBuilder =
                builder.DefineMethod("set_" + fieldBuilder.Name,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    null, new[] {fieldBuilder.FieldType});

            var setIL = setMethodBuilder.GetILGenerator();

            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);

            return setMethodBuilder;
        }
    }
}