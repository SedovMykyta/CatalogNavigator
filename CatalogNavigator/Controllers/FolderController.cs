using System.Globalization;
using System.IO.Compression;
using CatalogNavigator.Data;
using CatalogNavigator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogNavigator.Controllers;

public class FolderController : Controller
{
    private readonly CatalogNavigatorContext _context;

    public FolderController(CatalogNavigatorContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetListAsync()
    {
        var folders = await _context.Folders.ToListAsync();

        return View(folders);
    }

    [HttpGet]
    public IActionResult ImportFolder()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ImportFolder(IFormFile uploadedFile)
    {
        return RedirectToAction("Index", "Home");
    }

    //TODO
    public async Task<IActionResult> ExportFolder()
    {
        var folders = await  _context.Folders.ToListAsync();

        return View(folders);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadFolder(int id)
    {
        var folder = await _context.Folders.Include(f => f.Children).FirstOrDefaultAsync(f => f.Id == id);
        await LoadFolderTree(folder);
        
        if (folder == null)
        {
            throw new CultureNotFoundException();
        }
    
        var memoryStream = new MemoryStream();
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            CreateZipEntry(archive, folder);
        }
        memoryStream.Position = 0;
        return File(memoryStream, "application/zip", $"{folder.Name}.zip");
    }

    private async Task LoadFolderTree(Folder folder)
    {
        await _context.Entry(folder).Collection(f => f.Children).LoadAsync();
        foreach (var child in folder.Children)
        {
            await LoadFolderTree(child);
        }
    }

    private void CreateZipEntry(ZipArchive archive, Folder folder, string parentPath = "")
    {
        var folderPath = Path.Combine(parentPath, folder.Name);
        var folderEntry = archive.CreateEntry($"{folderPath}/");

        foreach (var child in folder.Children)
        {
            if (child.Children.Any())
            {
                CreateZipEntry(archive, child, folderPath);
            }
            else
            {
                var filePath = Path.Combine(folderPath, child.Name);

                if (!Path.HasExtension(filePath))
                {
                    var childFolderEntry = archive.CreateEntry($"{filePath}/");
                }
                else
                {
                    var fileEntry = archive.CreateEntry(filePath);

                    using (var writer = new StreamWriter(fileEntry.Open()))
                    {
                        // Write file content to writer
                    }
                }
            }
        }

        if (!folder.Children.Any())
        {
            var childFolderEntry = archive.CreateEntry($"{folderPath}/");
        }
    }
}