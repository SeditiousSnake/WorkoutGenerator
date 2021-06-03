using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Text;

namespace DatabaseTest
{
    public interface ISampleService
    {
        string GetCurrentDate();
        List<Exercise> GetExercisesFromExcel();
    }

    public class SampleService : ISampleService
    {
        public string GetCurrentDate() => System.DateTime.Now.ToLongDateString();

        public List<Exercise> GetExercisesFromExcel()
        {
            List<Exercise> exercises = new List<Exercise>();
            string currentPath = System.Environment.CurrentDirectory;
            Application excel = new Application();
            Workbook wb = excel.Workbooks.Open(currentPath + "\\Full Training Database.xlsx");
            Worksheet sheet1 = (Worksheet) wb.Worksheets[1];
            Range range = sheet1.UsedRange;
            object cellValue;
            int rCnt;
            int cCnt;
            int rw = 3;
            int cl = 1;

            range = sheet1.UsedRange;
            rw = range.Rows.Count;
            cl = range.Columns.Count;
            Exercise currentExercise;


            for (rCnt = 4; rCnt <= rw; rCnt++)
            {
                currentExercise = new Exercise();
                cellValue = (range.Cells[rCnt, 1] as Range).Value2;
                if (!(cellValue is null))
                {
                    for (cCnt = 1; cCnt <= cl; cCnt++)
                    {
                        cellValue = (range.Cells[rCnt, cCnt] as Range).Value2;
                        switch (cCnt)
                        {
                            case (1):
                                currentExercise.BodyPart = cellValue.ToString(); ;
                                break;
                            case (2):
                                currentExercise.Name = cellValue.ToString();
                                break;
                            case (3):
                                currentExercise.TargetArea = cellValue.ToString();
                                break;
                            case (4):
                                currentExercise.Type = cellValue.ToString();
                                break;
                            case (5):
                                currentExercise.Sets = cellValue.ToString();
                                break;
                            case (6):
                                currentExercise.Beginner = cellValue.ToString();
                                break;
                            case (7):
                                currentExercise.Normal = cellValue.ToString();
                                break;
                            case (8):
                                currentExercise.Don = cellValue.ToString();
                                break;
                            case (9):
                                currentExercise.DonHigh = cellValue.ToString();
                                break;
                            case (10):
                                currentExercise.Power = cellValue.ToString();
                                break;
                        }
                    }
                    exercises.Add(currentExercise);
                }
            }

            return exercises;
        }
    }
}
