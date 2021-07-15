using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WorkoutGenerator;
using WorkoutGenerator.UserControls;

namespace DatabaseTest
{
    public interface IExcelService
    {
        string GetCurrentDate();
        List<Exercise> GetExerciseDatabaseFromExcel();
        Task<List<WorkoutStep>> GenerateWorkoutForTemplate();
        Task ExportToExcel(List<PlanStep> planSteps);
        Task ExportToExcel(List<OutputStep> outputSteps);
    }

    public class ExcelService : IExcelService
    {
        public string GetCurrentDate() => System.DateTime.Now.ToLongDateString();

        public List<Exercise> GetExerciseDatabaseFromExcel()
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
                                currentExercise.BodyPart = cellValue.ToString().Trim();
                                break;
                            case (2):
                                currentExercise.Name = cellValue.ToString().Trim();
                                break;
                            case (3):
                                currentExercise.TargetArea = cellValue.ToString().Trim();
                                break;
                            case (4):
                                currentExercise.Type = cellValue.ToString().Trim();
                                break;
                            case (5):
                                currentExercise.Sets = cellValue.ToString().Trim();
                                break;
                            case (6):
                                currentExercise.Beginner = cellValue.ToString().Trim();
                                break;
                            case (7):
                                currentExercise.Normal = cellValue.ToString().Trim();
                                break;
                            case (8):
                                currentExercise.Don = cellValue.ToString().Trim();
                                break;
                            case (9):
                                currentExercise.DonHigh = cellValue.ToString().Trim();
                                break;
                            case (10):
                                currentExercise.Power = cellValue.ToString().Trim();
                                break;
                        }
                    }
                    exercises.Add(currentExercise);
                }
            }
            excel.Quit();
            Marshal.ReleaseComObject(sheet1);
            Marshal.ReleaseComObject(wb);
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(range);

            return exercises;
        }

        //TODO: Clean up this method to use a model, not a view. Fixing the binding for the exercise list might help.
        public async Task ExportToExcel(List<PlanStep> planSteps)
        {
            Application excel = new Application();
            Workbook wb = null;
            Worksheet sheet1 = null;

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
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
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

        public async Task<List<WorkoutStep>> GenerateWorkoutForTemplate()
        {
            List<WorkoutStep> steps = new List<WorkoutStep>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "xlsx";
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (openFileDialog.ShowDialog() == true)
            {
                Application excel = new Application();
                Workbook wb = excel.Workbooks.Open(openFileDialog.FileName);
                Worksheet sheet1 = (Worksheet)wb.Worksheets[1];
                Range range = sheet1.UsedRange;
                object cellValue;
                int rCnt;
                int cCnt;
                int rw;
                int cl;

                range = sheet1.UsedRange;
                rw = range.Rows.Count;
                cl = range.Columns.Count;
                WorkoutStep currentStep;

                for (rCnt = 4; rCnt <= rw; rCnt++)
                {
                    currentStep = new WorkoutStep();
                    cellValue = (range.Cells[rCnt, 6] as Range).Value2;
                    if (!(cellValue is null))
                    {
                        for (cCnt = 6; cCnt <= cl; cCnt++)
                        {
                            cellValue = (range.Cells[rCnt, cCnt] as Range).Value2;
                            switch (cCnt)
                            {
                                case (6):
                                    currentStep.BodyPart = cellValue.ToString().Trim(); ;
                                    break;
                                case (7):
                                    currentStep.TargetArea = cellValue.ToString().Trim();
                                    break;
                                case (8):
                                    currentStep.Type = cellValue.ToString().Trim();
                                    break;
                                case (9):
                                    currentStep.Sets = cellValue.ToString().Trim();
                                    break;
                                case (10):
                                    currentStep.Reps = cellValue.ToString().Trim();
                                    break;
                            }
                        }
                        steps.Add(currentStep);
                    }
                }

                excel.Quit();
                Marshal.ReleaseComObject(sheet1);
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(range);
            }

            return steps;
        }

        public async Task ExportToExcel(List<OutputStep> outputSteps)
        {
            Application excel = new Application();
            Workbook wb = null;
            Worksheet sheet1 = null;

            wb = excel.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            try
            {
                int row = 1;
                sheet1 = (Worksheet)wb.Worksheets[1];
                sheet1.Name = "Workout Plan";
                sheet1.Cells[row, 1] = "Exercise Name";
                sheet1.Cells[row, 2] = "Sets";
                sheet1.Cells[row, 3] = "Reps";
                var range = sheet1.get_Range("C1").EntireColumn;
                range.NumberFormat = "@";

                foreach (OutputStep step in outputSteps)
                {
                    row++;

                    sheet1.Cells[row, 1] = step.ExerciseName;
                    sheet1.Cells[row, 2] = step.NumberOfSets;
                    sheet1.Cells[row, 3] = step.NumberOfReps;
                }
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == true)
                {
                    wb.SaveAs(saveFileDialog.FileName);
                }

                excel.Quit();
                Marshal.ReleaseComObject(sheet1);
                Marshal.ReleaseComObject(wb);
                Marshal.ReleaseComObject(excel);
                Marshal.ReleaseComObject(range);
            }
            catch (System.Exception exHandle)
            {

                System.Console.WriteLine("Exception: " + exHandle.Message);

                System.Console.ReadLine();

            }
        }
    }
}
