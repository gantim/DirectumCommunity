using System.Text.RegularExpressions;

namespace DirectumCommunity.Models.ViewModels;

public class EmployeeInfoViewModel
{
    public Employee Employee { get; set; }
    public List<PersonChange> History { get; set; }
    public List<EmployeeChangeTableModel> TableModel { get; set; }
    public EmployeeLastnameChangeModel LastNameModel { get; set; }
    public string Period { get; set; }
    public string DismissalDate { get; set; }

    public void FillChanges()
    {
        FillTableModel();
        FillLastNameChanges();
        FillPeriod();
        FillDismissal();
    }

    private void FillDismissal()
    {
        if (!string.IsNullOrEmpty(Employee.Note) && Employee.Note.Contains("Дата увольнения"))
        {
            string pattern = @"(\d{2}.\d{2}.\d{4})";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(Employee.Note);

            if (match.Success)
            {
                string dateString = match.Groups[1].Value;

                if (DateTime.TryParseExact(dateString, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None,
                        out DateTime result))
                {
                    DismissalDate = result.ToShortDateString();
                }
                else
                {
                    DismissalDate = "-";
                }
            }
            else
            {
                DismissalDate = "-";
            }
        }
        else
        {
            DismissalDate = "-";
        }
    }
    
    public void FillPeriod()
    {
        var currentDate = DateTime.Today;
        if (History.Any())
        {
            var startDate = History.MinBy(h => h.ModifyDate)?.ModifyDate;
            Period = $"{startDate.Value.Date.ToShortDateString()} - {currentDate.ToShortDateString()}";
        }
        else
        {
            Period = $"{currentDate.ToShortDateString()} - {currentDate.ToShortDateString()}";
        }
    }
    
    private void FillLastNameChanges()
    {
        var lastNameChange = History.OrderByDescending(h => h.ModifyDate)
            .FirstOrDefault(h => h.Type == ChangeType.Lastname);
        if (lastNameChange != null)
        {
            LastNameModel = new EmployeeLastnameChangeModel()
            {
                OldValue = $"{lastNameChange.OldValue} {Employee.Person.FirstName} {Employee.Person.MiddleName}",
                NewValue =
                    $"{lastNameChange.NewValue} {Employee.Person.FirstName} {Employee.Person.MiddleName} (изменено {lastNameChange.ModifyDate.Date.ToShortDateString()})"
            };
        }
    }

    private void FillTableModel()
    {
        TableModel = new List<EmployeeChangeTableModel>();
        
        foreach (var item in History.Where(h => h.Type is ChangeType.Department or ChangeType.JobTitle))
        {
            var row = TableModel.FirstOrDefault(tm => tm.Date == item.ModifyDate.Date.ToShortDateString());
            if (row != null)
            {
                switch (item.Type)
                {
                    case ChangeType.Department:
                        row.NewDepartment = item.NewValue;
                        row.OldDepartment = item.OldValue;
                        break;
                    case ChangeType.JobTitle:
                        row.NewJobTitle = item.NewValue;
                        row.OldJobTitle = item.OldValue;
                        break;
                }
            }
            else
            {
                switch (item.Type)
                {
                    case ChangeType.Department:
                        TableModel.Add(new EmployeeChangeTableModel()
                        {
                            Date = item.ModifyDate.Date.ToShortDateString(),
                            NewDepartment = item.NewValue,
                            OldDepartment = item.OldValue
                        });
                        break;
                    case ChangeType.JobTitle:
                        TableModel.Add(new EmployeeChangeTableModel()
                        {
                            Date = item.ModifyDate.Date.ToShortDateString(),
                            NewJobTitle = item.NewValue,
                            OldJobTitle = item.OldValue
                        });
                        break;
                }
            }
        }

        TableModel = TableModel.OrderBy(tm => DateTime.Parse(tm.Date)).ToList();
    }
}

public class EmployeeChangeTableModel
{
    public string Date { get; set; }
    public string OldDepartment { get; set; }
    public string NewDepartment { get; set; }
    public string OldJobTitle { get; set; }
    public string NewJobTitle { get; set; }
}

public class EmployeeLastnameChangeModel
{
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}