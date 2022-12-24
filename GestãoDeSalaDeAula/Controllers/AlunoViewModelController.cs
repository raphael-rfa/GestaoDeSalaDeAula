﻿using GestãoDeSalaDeAula.Data;
using GestãoDeSalaDeAula.Models;
using GestãoDeSalaDeAula.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestãoDeSalaDeAula.Controllers
{
    public class AlunoViewModelController : Controller
    {
        private readonly GestãoDeSalaDeAulaContext? _context;

        public AlunoViewModelController(GestãoDeSalaDeAulaContext? context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(int Id)
        {

            AlunoViewModel aluno = await _context.Alunos.FindAsync(Id);            
            ViewData["MateriasId"] = new SelectList(_context.Materias, "Id", "MateriasName");
            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlunoViewModel notas)
        {
            if (ModelState.IsValid)
            {
                Provas provas = notas.provas;
                provas.AlunosId = notas.aluno.Id;
                _context!.Provas.Add(provas);
                await _context.SaveChangesAsync();
                ViewData["MateriasId"] = new SelectList(_context.Materias, "Id", "MateriasName", notas.provas.MateriasId);
                return View();
            }            
            ViewData["MateriasId"] = new SelectList(_context.Materias, "Id", "MateriasName", notas.provas.MateriasId);
            return View(notas);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Provas == null)
            {
                return NotFound();
            }

            AlunoViewModel prova = await _context.Provas
                .Include(a => a.Aluno)
                .Include(m => m.Materias)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProvasId == id);
            if (prova == null)
            {
                return NotFound();
            }
            return View(prova);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlunoViewModel ProvasAluno)
        {
            Provas prova = ProvasAluno.provas;
            prova.AlunosId = ProvasAluno.provas.Aluno.Id;
            if (id != prova.ProvasId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prova);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvaExists(prova.ProvasId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new RouteValueDictionary(
                    new { controller = "Alunoes", action = "Index" }));
            }
            return View(prova);
        }

        private bool ProvaExists(int id)
        {
            return (_context.Provas?.Any(e => e.ProvasId == id)).GetValueOrDefault();
        }

        //HTTP Get Delete        
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Provas == null)
            {
                return NotFound();
            }

            AlunoViewModel prova = await _context.Provas
                .Include(a => a.Aluno)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ProvasId == id);
            if (prova == null)
            {
                return NotFound();
            }
            return View(prova);
        }

        // POST: Alunoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(AlunoViewModel ProvasAluno)
        {
            Provas prova = ProvasAluno.provas;
            prova.AlunosId = ProvasAluno.provas.Aluno.Id;

            if (_context.Provas == null)
            {
                return Problem("Erro 'GestãoDeSalaDeAulaContext.Provas' é nulo.");
            }
            
            if (prova != null)
            {
                _context.Provas.Remove(prova);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Detail", new RouteValueDictionary(
                    new { controller = "Alunoes", action = "Index", prova.Aluno.Id }));
        } 
    }

}
