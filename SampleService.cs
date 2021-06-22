using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WorkoutGenerator.UserControls;

namespace DatabaseTest
{
    public interface ISampleService
    {
        string GetCurrentDate();
        List<Exercise> GetExercisesFromExcel();
        Task ExportToExcel(List<PlanStep> planSteps);
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

        //TODO: Clean up this method to use a model, not a view. Fixing the binding for the exercise list might help.
        public async Task ExportToExcel(List<PlanStep> planSteps)
        {
            Application excel = new Application();
            Workbook wb = null;
            Worksheet sheet1 = null;

            excel.Visible = true;
            wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            try
            {
                int row = 1;
                sheet1 = (Worksheet) wb.Worksheets[1];
                sheet1.Name = "Workout Plan";
                sheet1.Cells[row, 1] = "Exercise Name";
                sheet1.Cells[row, 2] = "Body Part";
                sheet1.Cells[row, 3] = "Target Area";
                sheet1.Cells[row, 4] = "Type";
                sheet1.Cells[row, 5] = "Sets";
                sheet1.Cells[row, 6] = "Reps";

                foreach (PlanStep step in planSteps)
                {
                    row++;

                    sheet1.Cells[row, 1] = step.Exercise.Name;
                    sheet1.Cells[row, 2] = step.Exercise.BodyPart;
                    sheet1.Cells[row, 3] = step.Exercise.TargetArea;
                    sheet1.Cells[row, 4] = step.Exercise.Type;
                    sheet1.Cells[row, 5] = step.Exercise.Sets;
                    sheet1.Cells[row, 6] = step.Reps;
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "xlsx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    wb.SaveAs(saveFileDialog.FileName);
                }

                excel.Quit();
                Marshal.ReleaseComObject(sheet1);
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(excel);
            }
            catch (System.Exception exHandle)
            {

                System.Console.WriteLine("Exception: " + exHandle.Message);

                System.Console.ReadLine();

            }
        }
    }
}
