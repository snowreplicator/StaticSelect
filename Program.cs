using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ru.zorro.static_select;
using System.Diagnostics;
//using System.Linq.Dynamic;


/*
select lessons.* 
from
  classifiers.classifier cl
inner join classifiers.classifierset cl_set
        on cl.classifierset_id = cl_set.classifierset_id
        inner join multi_d_cases."LessonSource" lessons
                on cl.value::INTEGER = lessons."LessonSource_id"
where
    cl_set.classnamepk like '%LessonSource%'
 and
    cl.deleted = false
order by
  lessons.code desc,
  lessons."Name" asc
*/

namespace ru.zorro.static_select
{
    public class Program
    {
        private static readonly string DB_CONNECTION_STRING = "Server=127.0.0.1;Port=5432;Database=visual_studio;Username=portal;Password=troP4444";

        static void Main(string[] args)
        {
            ApplicationContext applicationContext = new ApplicationContext(DB_CONNECTION_STRING);
            // all data
            //AllDataInLesonSourceTable(applicationContext);
            //AllDataInDatatypeTestSourceTable(applicationContext);

            // select and join
            //AllActiveDataByCustomSql(applicationContext);
            //AllLessonAndClassifierByQuery(applicationContext);
            //AllActiveDataByLinQ(applicationContext);

            // filter
            //LinqSelectAndOrdering(applicationContext);
            // search
            //LinqSelectAndSearchPredicates(applicationContext);
            LinqSelectAndSearch(applicationContext);

            // LocalTableInsert(applicationContext);

            applicationContext.Dispose();
        }










        private static void LinqSelectAndSearch(ApplicationContext applicationContext)
        {

            // выборка и последовательная сортировка по массиву указанных полей
            bool deleted = false;
            string className = "DatatypeTest";
            var sortProperties = new string[] { "DatatypeBool", "DatatypeInt", "DatatypeString" };
            var sortDescProps = new bool[] { true, true, true };
            string search = "True";
            string search2 = "8000";
            string search3 = "0001";
            string search4 = "0003";


            Type modelType = typeof(DatatypeTests);
            //var models2 = applicationContext.Where(modelType, x => x.GetType().Name != null).ToList();

            /*
            var predicate = PredicateBuilder.False<DatatypeTest>();
            foreach (string keyword in sortProperties)
                predicate = predicate.Or(p => p.DatatypeString.Contains(search4));*/


            /*
            var predicate = PredicateBuilder.False<DatatypeTest>();
            predicate = predicate.Or(p => p.DatatypeString.Contains(search4));

            var predicate2 = PredicateBuilder.False<DatatypeTest>();
            predicate2 = predicate2.Or(p => p.DatatypeInt.ToString().Equals(search2));

            var predicate3 = PredicateBuilder.True<DatatypeTest>();
            predicate3 = predicate3.And(p => p.DatatypeString.Contains(search4));

            var predicate4 = PredicateBuilder.True<DatatypeTest>();
            predicate4 = predicate4.And(p => p.DatatypeInt.ToString().Equals(search2));

            var models = applicationContext.DatatypeTests
                //.Where(x => x.DatatypeBool == true)
                //.Where(x => x.GetType(). .DatatypeBool == true)
                //.Select(x => x.GetType().Name)
                //.Where(models => type.IsAssignableFrom(models.GetType()))
                //.FilterByBool(false)
                //.FilterByBool(false)
                //.FilterByProps(search2, sortProperties)
                .Where(predicate)
                //.Where(predicate2)
                //.Where(predicate3)
                //.Where(predicate4)
                .ToList();

            // var list2 = context.Customers.Execute<IEnumerable<Customer>>("Where(x => x.IsActive == IsActive)", new { IsActive = false }).ToList();
            //var list2 = applicationContext.DatatypeTests.Exe
            //.Execute<IEnumerable<Customer>>("Where(x => x.IsActive == IsActive)", new { IsActive = false }).ToList();

            //var predicate = PredicateBuilder.Or("c", "d");


            Console.WriteLine("\n selected, searced and sorted models :\n");
            foreach (var model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */


            // https://stackoverflow.com/questions/14901430/building-dynamic-where-clauses-in-linq-to-ef-queries
            // https://www.roundthecode.com/dotnet/entity-framework/using-linq-expressions-to-build-dynamic-queries-in-entity-framework
            // https://habr.com/ru/post/181065/
            // https://pranayamr.blogspot.com/2011/04/dynamic-query-with-linq.html





            // http://sergeyteplyakov.blogspot.com/2010/12/dynamic-linq.html
            // http://www.albahari.com/nutshell/predicatebuilder.aspx
            // http://www.karanowood.com/Dynamic%20Expressions.html#_Dynamic_Expression_API
            var x = Expression.Parameter(typeof(int), "xxxx");
            var expression = Expression.Lambda<Func<int, int>>(
                Expression.Add(
                    x,
                    Expression.Constant(1)
                ),
                x
                );



            /*
            var xExpression = Expression.Parameter(typeof(int), "x");
            var parsedExpression =
                (Expression<Func<int, int>>)DynamicExpression.ParseLambda(
                new[] { xExpression }, null, "x + 1");*/
            /*
            Expression<Func<DatatypeTest, bool>> e1 =
    DynamicExpression.ParseLambda<DatatypeTest, bool>("City = \"London\"");
            Expression<Func<Customer, bool>> e2 =
                DynamicExpression.ParseLambda<Customer, bool>("Orders.Count >= 10");
            IQueryable<Customer> query =
                db.Customers.Where("@0(it) and @1(it)", e1, e2);

            var query =
    applicationContext.DatatypeTests.
    Where("City = @0 and Orders.Count >= @1", "London", 10).
    OrderBy("CompanyName").
    Select("new(CompanyName as Name, Phone)");*/


            sortProperties = new string[] { "DatatypeBool", "DatatypeInt", "DatatypeString" };
            sortDescProps = new bool[] { true, true, true };



            var predicate = PredicateBuilder.False<DatatypeTests>();
            predicate = predicate.Or(p => p.DatatypeString.Contains(search4));

            var predicate2 = PredicateBuilder.False<DatatypeTests>();
            predicate2 = predicate2.Or(p => p.DatatypeInt.ToString().Equals(search2));



            var predicate33 = PredicateBuilder.True<DatatypeTests>();
            predicate33 = predicate33.And(e1 => e1.DatatypeString.Contains("0001"));
            predicate33 = predicate33.And(e1 => e1.DatatypeInt == 9001);
            predicate33 = predicate33.Or(e1 => e1.DatatypeInt == 1111);

            var models = applicationContext.DatatypeTests
            //.Where(x => x.DatatypeBool == true)
            //.Where(x => x.GetType(). .DatatypeBool == true)
            //.Select(x => x.GetType().Name)
            //.Where(models => type.IsAssignableFrom(models.GetType()))
            //.FilterByBool(false)
            //.FilterByBool(false)
            //.FilterByProps(search2, sortProperties)
            //.Where(predicate)
            //.Where(predicate2)
            //.Where(predicate2)
            //.Where(x => x.GetType().GetProperty("DatatypeString").GetValue(x) == 3 )


            // https://stackoverflow.com/questions/68737681/the-linq-expression-could-not-be-translated-either-rewrite-the-query-in-a-form
            // .ToList()
            //.Where<DatatypeTest>(x => x.GetType().GetProperty("DatatypeString").GetValue(x, null).Equals("строка номер 0001"))

            //        .WhereHelper2("DatatypeBool", true)
            //.Where(predicate33)
            //.OrderingHelper<DatatypeTests>(sortProperties, sortDescProps)
            //.Where(predicate3)
            //.Where(predicate4)
            .TextFilter("0003")
            //    .Select(CreateNewStatement("DatatypetestId, DatatypeString"))
               .ToList();


            Console.WriteLine("\n selected, searced and sorted models :\n");
            foreach (var model in models)
              Console.WriteLine(JsonConvert.SerializeObject(model));



            var x2 = Expression.Parameter(typeof(int), "xx => xx.DatatypeInt == 8000");
            var expression2 = Expression.Lambda<Func<int, int>>(
                Expression.Add(
                    x2,
                    Expression.Constant(0)
                ),
                x
                );


            Console.WriteLine("the end");
            // https://stackoverflow.com/questions/3932542/how-to-dynamically-build-and-return-a-linq-predicate-based-on-user-input

        }






        public static Func<DatatypeTests, DatatypeTests> CreateNewStatement(string fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(DatatypeTests), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(DatatypeTests));

            // create initializers
            var bindings = fields.Split(',').Select(o => o.Trim())
                .Select(o => {

            // property "Field1"
            var mi = typeof(DatatypeTests).GetProperty(o);

            // original value "o.Field1"
            var xOriginal = Expression.Property(xParameter, mi);

            // set value "Field1 = o.Field1"
            return Expression.Bind(mi, xOriginal);
                }
            );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<DatatypeTests, DatatypeTests>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();
        }





















        private static void LinqSelectAndSearchPredicates(ApplicationContext applicationContext)
        {

            // выборка и последовательная сортировка по массиву указанных полей
            bool deleted = true;
            string className = "DatatypeTest";
            var sortProperties = new string[] { "DatatypeBool", "DatatypeInt", "DatatypeString" };
            var sortDescProps = new bool[] { true, true, true };


            var predicate = PredicateBuilder.True<DatatypeTests>();
            predicate = predicate.And(e1 => e1.DatatypeString.Contains("0001"));
            predicate = predicate.And(e1 => e1.DatatypeInt == 9001);
            predicate = predicate.Or(e1 => e1.DatatypeInt == 1111);


            var models = applicationContext.DatatypeTests
                // соединение DatatypeTests с Classifiers
                .Join(applicationContext.Classifiers,
                data_type => data_type.DatatypetestId.ToString(),
                classifier => classifier.Value,
                (data_type, classifier) => new { data_type, classifier })
                // условие соединения - берем все или только неудаленные записи
                .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
                // соединение Classifiers с Classifiersets
                .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
                // условие соединения - Classnamepk нужного справочника а само соединения было по id
                .Where(join => join.classifier_set.Classnamepk.Equals(className))
                // выборка данных из объединения
                .Select(join => join.classifier.data_type)
                .Where(predicate)                
                // сортировка данных из объединения
                //.OrderingHelper<DatatypeTest>(propertyName, desc, false)
                //.OrderingHelper<DatatypeTest>(propertyName2, desc2, true)
                //.OrderingHelper<DatatypeTest>(propertyName3, desc3, true)
                .OrderingHelper<DatatypeTests>(sortProperties, sortDescProps)
                .ToList();


            Console.WriteLine("\n selected and sorted models with predicate :\n");
            foreach (var model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));


        }












        private static void LinqSelectAndOrdering(ApplicationContext applicationContext)
        {
            // выборка DatatypeTest со связкой на Classifiers и на Classifiersets
            string className = "DatatypeTest";
            bool deleted = true;


            /*
            var models = applicationContext.DatatypeTests.Join(applicationContext.Classifiers,
                 data_type => data_type.DatatypetestId.ToString(),
                 classifier => classifier.Value,
                 (data_type, classifier) => new { data_type, classifier })
           .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
           .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
           .Where(join => join.classifier_set.Classnamepk.Equals(className))
           .Select(join => join.classifier.data_type)
           .ToList();

            Console.WriteLine("\n select and search:\n");
            foreach (var model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */


            /*
            // ------------------------ 
            // https://entityframework.net/knowledge-base/45855152/entity-framework-linq-orderby-function
            // https://habr.com/ru/sandbox/94173/
            string propertyName = "DatatypeBool";
            string propertyName2 = "DatatypeInt";
            bool asc = false;
            bool asc2 = false;

            var models2 = models.OrderByDynamic(propertyName, asc);
            Console.WriteLine("\n sorted models 2:\n");
            foreach (var model in models2)
                Console.WriteLine(JsonConvert.SerializeObject(model));

            var models3 = models2.OrderByDynamic(propertyName2, asc2);
            Console.WriteLine("\n sorted models 3:\n");
            foreach (var model in models3)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            // --------------------------------------------
            */




            /*
            // применение сортировки по 1ному столбцу на выборке
            string propertyName = "DatatypeBool";
            string propertyName2 = "DatatypeInt";
            bool asc = false;
            bool asc2 = false;

            var models = applicationContext.DatatypeTests.Join(applicationContext.Classifiers,
                 data_type => data_type.DatatypetestId.ToString(),
                 classifier => classifier.Value,
                 (data_type, classifier) => new { data_type, classifier })
           .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
           .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
           .Where(join => join.classifier_set.Classnamepk.Equals(className))
           .Select(join => join.classifier.data_type)
           .OrderByDynamic(propertyName, asc)
           .OrderByDynamic(propertyName2, asc2)  // последний orderBy перебарывает предыдущие orderby
           .ToList();

            Console.WriteLine("\n sorted models :\n");
            foreach (var model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */





            /*
            // создать Expression
            bool desc = true;
            var models2 = OrderingHelper<DatatypeTest>(applicationContext.DatatypeTests, propertyName, desc, false).ToList();
            Console.WriteLine("\n sorted models 2:\n");
            foreach (var model in models2)
                Console.WriteLine(JsonConvert.SerializeObject(model));*/



            /*
            string propertyName = "DatatypeBool";
            string propertyName2 = "DatatypeInt";
            bool desc = true;
            bool desc2 = false;

            var models = applicationContext.DatatypeTests.Join(applicationContext.Classifiers,
                 data_type => data_type.DatatypetestId.ToString(),
                 classifier => classifier.Value,
                 (data_type, classifier) => new { data_type, classifier })
           .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
           .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
           .Where(join => join.classifier_set.Classnamepk.Equals(className))
           .Select(join => join.classifier.data_type);

            var models2 = models.OrderingHelper<DatatypeTest>(propertyName, desc, false)
                .OrderingHelper<DatatypeTest>(propertyName2, desc2, true)
                .ToList();

            Console.WriteLine("\n sorted models 2:\n");
            foreach (var model in models2)
                Console.WriteLine(JsonConvert.SerializeObject(model)); 
            */

            /*
            // выборка и последовательная сортировка по 3м полям
            deleted = false;
            string propertyName = "DatatypeBool";
            string propertyName2 = "DatatypeInt";
            string propertyName3 = "DatatypeString";
            bool desc = true;
            bool desc2 = true;
            bool desc3 = true;

            var models = applicationContext.DatatypeTests
                // соединение DatatypeTests с Classifiers
                .Join(applicationContext.Classifiers,
                data_type => data_type.DatatypetestId.ToString(),
                classifier => classifier.Value,
                (data_type, classifier) => new { data_type, classifier })
                // условие соединения - берем все или только неудаленные записи
                .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
                // соединение Classifiers с Classifiersets
                .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
                // условие соединения - Classnamepk нужного справочника а само соединения было по id
                .Where(join => join.classifier_set.Classnamepk.Equals(className))
                // выборка данных из объединения
                .Select(join => join.classifier.data_type)
                // сортировка данных из объединения
                //.OrderingHelper<DatatypeTest>(propertyName, desc, false)
                //.OrderingHelper<DatatypeTest>(propertyName2, desc2, true)
                //.OrderingHelper<DatatypeTest>(propertyName3, desc3, true)
                .OrderingHelper<DatatypeTest>(new string[] { "DatatypeBool", "DatatypeInt", "DatatypeString" }, new bool[] { true, true, true })
                .ToList();



            Console.WriteLine("\n sorted models :\n");
            foreach (var model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */



            // выборка и последовательная сортировка по массиву указанных полей
            deleted = false;
            className = "DatatypeTest";
            var sortProperties = new string[] { "DatatypeBool", "DatatypeInt", "DatatypeString" };
            var sortDescProps = new bool[] { true, true, true };


            var models = applicationContext.DatatypeTests
                // соединение DatatypeTests с Classifiers
                .Join(applicationContext.Classifiers,
                data_type => data_type.DatatypetestId.ToString(),
                classifier => classifier.Value,
                (data_type, classifier) => new { data_type, classifier })
                // условие соединения - берем все или только неудаленные записи
                .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
                // соединение Classifiers с Classifiersets
                .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
                // условие соединения - Classnamepk нужного справочника а само соединения было по id
                .Where(join => join.classifier_set.Classnamepk.Equals(className))
                // выборка данных из объединения
                .Select(join => join.classifier.data_type)
                // сортировка данных из объединения
                //.OrderingHelper<DatatypeTest>(propertyName, desc, false)
                //.OrderingHelper<DatatypeTest>(propertyName2, desc2, true)
                //.OrderingHelper<DatatypeTest>(propertyName3, desc3, true)
                .OrderingHelper<DatatypeTests>(sortProperties, sortDescProps)
                .ToList();


            Console.WriteLine("\n selected and sorted models :\n");
            foreach (var model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));

        }



        /// <summary>
        /// Сортировка
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="source">Источник данных</param>
        /// <param name="propertyName">Имя поля</param>
        /// <param name="descending">По возрастающий</param>
        /// <param name="anotherLevel"></param>
        /// <returns>Отсортированный запрос либо null(В случае ошибки)</returns>
        private static IQueryable<T> OrderingHelperStatic<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            if (!string.IsNullOrEmpty(propertyName))
                try
                {
                    string sss = (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty);
                    // ---------------
                    ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
                    MemberExpression property = Expression.PropertyOrField(param, propertyName);
                    LambdaExpression sort = Expression.Lambda(property, param);

                    MethodCallExpression call = Expression.Call(
                        typeof(Queryable),
                        (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                        new[] { typeof(T), property.Type },
                        source.Expression,
                        Expression.Quote(sort));
                    return (IQueryable<T>)source.Provider.CreateQuery<T>(call);
                }
                catch
                {
                    return null;
                }
            return null;
        }

















        // выборка данных через использование Linq
        // https://metanit.com/sharp/efcore/3.3.php
        // https://metanit.com/sharp/efcore/5.3.php
        private static void AllActiveDataByLinQ(ApplicationContext applicationContext)
        {
            string className = "LessonSource";
            bool deleted = false;

            Console.WriteLine("\n data in lesson source table by linq (deleted = {0}):\n", deleted);


            /*
            // выборка Classifiersets со связкой на Classifiers
            var models1 = applicationContext.Classifiersets.Join(
                applicationContext.Classifiers,
                classifier_set => classifier_set.ClassifiersetId,
                classifier => classifier.ClassifiersetId,
                (classifier_sets, classifier) => classifier_sets)
                .Where(classifier_sets => classifier_sets.Classnamepk.Equals(className))
                .ToList();

            Console.WriteLine("\n list 1:\n");
            foreach (var model in models1)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */


            /*
            // выборка Classifiers со связкой на Classifiersets с учетом признака удаленности
            var models2 = applicationContext.Classifiers.Join(
                applicationContext.Classifiersets,
                classifier => classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_sets) => classifier)
                .Where(classifier => classifier.ClassifiersetId == 1 &&
                (deleted == false ? classifier.Deleted == false : classifier.Deleted == true || classifier.Deleted == false))
                .ToList();

            Console.WriteLine("\n list 2:\n");
            foreach (var model in models2)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */



            /*
            // выборка Classifiers со связкой на Classifiersets с учетом признака удаленности со всеми связываемыми параметрами
            var models3 = applicationContext.Classifiers.Join(applicationContext.Classifiersets,
                 classifier => classifier.ClassifiersetId,
                 classifier_set => classifier_set.ClassifiersetId,
                 (classifier, classifier_set) => new { classifier, classifier_set })
           .Where(sc => sc.classifier_set.Classnamepk.Equals(className) &&
                 (deleted == false ? sc.classifier.Deleted == false : sc.classifier.Deleted == true || sc.classifier.Deleted == false))
           .Select(sc => sc.classifier)
           .ToList();

            Console.WriteLine("\n list 3:\n");
            foreach (var model in models3)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            */



            // выборка LessonSource со связкой на Classifiers и на Classifiersets
            var models4 = applicationContext.LessonSources.Join(applicationContext.Classifiers,
                 lesson => lesson.LessonSourceId.ToString(),
                 classifier => classifier.Value,
                 (lesson, classifier) => new { lesson, classifier })
           .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
           .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
           .Where(join => join.classifier_set.Classnamepk.Equals(className))
           .Select(join => join.classifier.lesson)
           .ToList();

            Console.WriteLine("\n list 4:\n");
            foreach (var model in models4)
                Console.WriteLine(JsonConvert.SerializeObject(model));



            // выборка DatatypeTest со связкой на Classifiers и на Classifiersets
            className = "DatatypeTest";
            deleted = true;

            var models5 = applicationContext.DatatypeTests.Join(applicationContext.Classifiers,
                 data_type => data_type.DatatypetestId.ToString(),
                 classifier => classifier.Value,
                 (data_type, classifier) => new { data_type, classifier })
           .Where(join => (deleted == false ? join.classifier.Deleted == false : join.classifier.Deleted == true || join.classifier.Deleted == false))
           .Join(applicationContext.Classifiersets,
                classifier => classifier.classifier.ClassifiersetId,
                classifier_set => classifier_set.ClassifiersetId,
                (classifier, classifier_set) => new { classifier, classifier_set })
           .Where(join => join.classifier_set.Classnamepk.Equals(className))
           .Select(join => join.classifier.data_type)
           .ToList();

            Console.WriteLine("\n list 5:\n");
            foreach (var model in models5)
                Console.WriteLine(JsonConvert.SerializeObject(model));
            return;
        }



        // выбора данных через использование query
        // https://docs.microsoft.com/ru-ru/dotnet/csharp/linq/perform-inner-joins
        private static void AllLessonAndClassifierByQuery(ApplicationContext applicationContext)
        {
            Console.WriteLine("\n all lesson and classifier:\n");
            int classifierSetId = 1;
            bool delete = false;
            string className = "LessonSource";


            // соединение на 2 таблицы
            var models = from lessonSource in applicationContext.LessonSources
                         join classifier in applicationContext.Classifiers
                         on new
                         {
                             test1 = lessonSource.LessonSourceId.ToString(),
                             test2 = classifierSetId
                         }
                         equals new
                         {
                             test1 = classifier.Value,
                             test2 = classifier.ClassifiersetId
                         }
                         //where classifier.Deleted == delete
                         where (delete == false ? classifier.Deleted == false : classifier.Deleted == true || classifier.Deleted == false)
                         select lessonSource;
            /*{
                lessonid = lessonSource.LessonSourceId,
                classifierId = classifier.ClassifierId,
                classifier_classifiersetId = classifier.ClassifiersetId,
                value = classifier.Value
            };*/
            Console.WriteLine("\n list 1:\n");
            foreach (object model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));


            // соединение на 3 таблицы
            // https://stackoverflow.com/questions/21051612/entity-framework-join-3-tables
            var models2 = from lessonSource in applicationContext.LessonSources
                          join classifier in applicationContext.Classifiers
                          on new
                          {
                              test1 = lessonSource.LessonSourceId.ToString(),
                              test2 = classifierSetId
                          }
                          equals new
                          {
                              test1 = classifier.Value,
                              test2 = classifier.ClassifiersetId
                          }
                          join classifier_set in applicationContext.Classifiersets
                          on new
                          {
                              test3 = classifier.ClassifiersetId,
                          }
                          equals new
                          {
                              test3 = classifier_set.ClassifiersetId
                          }
                          where (delete == false ? classifier.Deleted == false : classifier.Deleted == true || classifier.Deleted == false) &&
                                 classifier_set.Classnamepk == className
                          //select classifier_set;
                          //select classifier;
                          //select lessonSource;
                          select new
                          {
                              classifier_set,
                              classifier,
                              lessonSource
                          };

            Console.WriteLine("\n list 2:\n");
            foreach (object model in models2)
                Console.WriteLine(JsonConvert.SerializeObject(model));

        }




        // выборка неудаленных данных из LessonSource и Classifier custom sql
        // https://docs.microsoft.com/ru-ru/dotnet/framework/data/adonet/cross-table-queries-linq-to-dataset
        // https://metanit.com/sharp/entityframework/4.4.php
        // https://metanit.com/sharp/efcore/6.1.php
        private static void AllActiveDataByCustomSql(ApplicationContext applicationContext)
        {
            Console.WriteLine("\n data in lesson source table by custom sql:\n");
            string query = "select lessons.*                                                  " +
                           "from                                                              " +
                           "  classifiers.classifier cl                                       " +
                           "inner join classifiers.classifierset cl_set                       " +
                           "        on cl.classifierset_id = cl_set.classifierset_id          " +
                           "        inner join multi_d_cases.\"LessonSource\" lessons         " +
                           "                on cl.value::INTEGER = lessons.\"LessonSource_id\"" +
                           "where                                                             " +
                           "    cl_set.classnamepk like '%LessonSource%'                      " +
                           " and                                                              " +
                           "    cl.deleted = false                                            " +
                           "order by                                                          " +
                           "  lessons.code desc,                                              " +
                           "  lessons.\"Name\" asc                                            ";

            var models = applicationContext.LessonSources.FromSqlRaw(query).ToList();
            foreach (LessonSource model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));

        }

        // выборка из таблицы справочника всех записей
        private static void AllDataInLesonSourceTable(ApplicationContext applicationContext)
        {
            Console.WriteLine("\nall data in lesson source table:\n");


            List<LessonSource> models = applicationContext.LessonSources.ToList();
            foreach (LessonSource model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
        }

        private static void AllDataInDatatypeTestSourceTable(ApplicationContext applicationContext)
        {
            Console.WriteLine("\nall data in datatypetestt source table:\n");


            List<DatatypeTests> models = applicationContext.DatatypeTests.ToList();
            foreach (DatatypeTests model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
        }


        private static void LocalTableInsert(ApplicationContext applicationContext)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < 1000; i++)
            {
                using (var dbContextTransaction = applicationContext.Database.BeginTransaction())
                {
                    if (i % 1000 == 0) Console.WriteLine("######i " + i);

                    var lesson = new LessonSource()
                    {
                        Name = "local insert: " + i,
                        Code = "local code: " + i
                    };

                    applicationContext.AddEntity(lesson);
                    applicationContext.SaveChanges();

                    dbContextTransaction.Commit();
                }
            }

            stopWatch.Stop();
            Console.WriteLine("####################################### " + stopWatch.ElapsedMilliseconds);
        }
    }
}
















// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(7, 'строка номер 0001', 7000, 666.66, true, '2021-11-26 22:26:26.666');
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(8, 'строка номер 0002', 7001, 666.66, true, '2021-11-26 22:26:26.666');
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(9, 'строка номер 0003', 7000, 666.66, true, '2021-11-26 22:26:26.666');
// 
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(10, 'строка номер 0001', 8000, 666.66, true, '2021-11-26 22:26:26.666');
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(11, 'строка номер 0002', 8001, 666.66, true, '2021-11-26 22:26:26.666');
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(12, 'строка номер 0003', 8000, 666.66, true, '2021-11-26 22:26:26.666');
// 
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(13, 'строка номер 0001', 9001, 666.66, false, '2021-11-26 22:26:26.666');
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(14, 'строка номер 0002', 9000, 666.66, false, '2021-11-26 22:26:26.666');
// INSERT INTO multi_d_cases."DatatypeTest" (datatypetest_id, datatype_string, datatype_int, datatype_double, datatype_bool, datatype_date) 
//     VALUES(15, 'строка номер 0003', 9000, 666.66, false, '2021-11-26 22:26:26.666');
// 
// 
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(50, 5, '7', false);
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(51, 5, '8', false);
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(52, 5, '9', false);
// 
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(53, 5, '10', false);
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(54, 5, '11', false);
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(55, 5, '12', false);
// 
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(56, 5, '13', false);
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(57, 5, '14', false);
// INSERT INTO classifiers.classifier (classifier_id, classifierset_id, value, deleted) VALUES(58, 5, '15', false);