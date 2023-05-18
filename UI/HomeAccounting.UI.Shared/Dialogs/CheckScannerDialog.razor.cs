using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace HomeAccounting.UI.Shared.Dialogs;

public partial class CheckScannerDialog
{
    private static readonly string DefaultDragClass
        = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full z-10 dragable-area";

    private readonly List<IBrowserFile> _files = new();

    private string _dragClass = DefaultDragClass;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    private void OnInputFileChanged(InputFileChangeEventArgs e)
    {
        ClearDragClass();
        var files = e.GetMultipleFiles();

        foreach (var file in files)
        {
            if (_files.Any(x => x.Name == file.Name))
            {
                continue;
            }

            var postedFileExtension = Path.GetExtension(file.Name);

            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                Snackbar.Add($"File {file.Name} ignored because this is not image", Severity.Warning);

                continue;
            }

            if (file.Size > 10 * 1024 * 1024)
            {
                Snackbar.Add($"Large file {file.Name} ignored", Severity.Warning);
            }

            _files.Add(file);
        }
    }

    private async Task ClearAsync()
    {
        _files.Clear();
        ClearDragClass();
        await Task.Delay(100);
    }

    private void Upload()
    {
        MudDialog.Close(DialogResult.Ok(_files));
    }

    private void SetDragClass()
    {
        _dragClass = $"{DefaultDragClass} mud-border-primary";
    }

    private void ClearDragClass()
    {
        _dragClass = DefaultDragClass;
    }
}