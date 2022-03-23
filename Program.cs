using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;


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
            AllDataInDatatypeTestSourceTable(applicationContext);

            // select and join
            //AllActiveDataByCustomSql(applicationContext);
            //AllLessonAndClassifierByQuery(applicationContext);
            //AllActiveDataByLinQ(applicationContext);

            // filter
            LinqSelectAndOrdering(applicationContext);

            applicationContext.Dispose();
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


            // сортировка по 3м полям
            deleted = false;
            string propertyName = "DatatypeBool";
            string propertyName2 = "DatatypeInt";
            string propertyName3 = "DatatypeString";
            bool desc = true;
            bool desc2 = true;
            bool desc3 = false;

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
                .OrderingHelper<DatatypeTest>(propertyName, desc, false)
                .OrderingHelper<DatatypeTest>(propertyName2, desc2, true)
                .OrderingHelper<DatatypeTest>(propertyName3, desc3, true)
                .ToList();


            Console.WriteLine("\n sorted models :\n");
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


            List<DatatypeTest> models = applicationContext.DatatypeTests.ToList();
            foreach (DatatypeTest model in models)
                Console.WriteLine(JsonConvert.SerializeObject(model));
        }
    }
}