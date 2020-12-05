using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TesteTimipro.Models;
using TesteTimipro.Models.Utils;

namespace TesteTimipro.Controllers
{
    public class PessoasController : Controller
    {
        private TimiproDB db = new TimiproDB();

        // GET: Pessoas
        public async Task<ActionResult> Index()
        {
            var tabelaPessoas = db.TabelaPessoas.Where(x => x.Deletado == false).Include(p => p.Produto);
            return View(await tabelaPessoas.ToListAsync());
        }

        // GET: Pessoas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = await db.TabelaPessoas.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // GET: Pessoas/Create
        public ActionResult Create()
        {
            ViewBag.ProdutoID = new SelectList(db.TabelaProdutos.Where(x => x.Deletado == false && x.Ativo == true), "ID", "Nome");
            return View();
        }

        // POST: Pessoas/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Pessoa pessoa)
        {
            if (CpfCheck(pessoa.CPF, pessoa.ID) == false || EmailCheck(pessoa.Email, pessoa.ID) == false)
            {
                ViewBag.ProdutoID = new SelectList(db.TabelaProdutos.Where(x => x.Deletado == false && x.Ativo == true), "ID", "Nome", pessoa.ProdutoID);
                TempData["MENSAGEM"] = new MensagemAviso { Tipo = 2, Descricao = "JÁ EXISTE PESSOA ASSOCIADO A ESTE CPF/EMAIL" };
                return View(pessoa);
            }
            if (ProdutoCheck(pessoa.ProdutoID, pessoa.ID) == false)
            {
                ViewBag.ProdutoID = new SelectList(db.TabelaProdutos.Where(x => x.Deletado == false && x.Ativo == true), "ID", "Nome");
                TempData["MENSAGEM"] = new MensagemAviso { Tipo = 2, Descricao = "PRODUTO SELECIONADO ESTÁ ASSOCIADO A OUTRA PESSOA!" };
                return View(pessoa);
            }
            db.TabelaPessoas.Add(pessoa);
            await db.SaveChangesAsync();
            TempData["MENSAGEM"] = new MensagemAviso { Tipo = 1, Descricao = pessoa.Nome + " CADASTRADO COM SUCESSO!" };
            return RedirectToAction("Index");
        }

        // GET: Pessoas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = await db.TabelaPessoas.FindAsync(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProdutoID = new SelectList(db.TabelaProdutos.Where(x => x.Deletado == false && x.Ativo == true), "ID", "Nome", pessoa.ProdutoID);
            return View(pessoa);
        }

        // POST: Pessoas/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Pessoa pessoa)
        {
            if (CpfCheck(pessoa.CPF, pessoa.ID) == false || EmailCheck(pessoa.Email, pessoa.ID) == false)
            {
                ViewBag.ProdutoID = new SelectList(db.TabelaProdutos.Where(x => x.Deletado == false && x.Ativo == true), "ID", "Nome", pessoa.ProdutoID);
                TempData["MENSAGEM"] = new MensagemAviso { Tipo = 2, Descricao = "JÁ EXISTE PESSOA ASSOCIADO A ESTE CPF/EMAIL" };
                return View(pessoa);
            }
            if (ProdutoCheck(pessoa.ProdutoID, pessoa.ID) == false)
            {
                ViewBag.ProdutoID = new SelectList(db.TabelaProdutos.Where(x => x.Deletado == false && x.Ativo == true), "ID", "Nome");
                TempData["MENSAGEM"] = new MensagemAviso { Tipo = 2, Descricao = "PRODUTO SELECIONADO ESTÁ ASSOCIADO A OUTRA PESSOA!" };
                return View(pessoa);
            }
            Pessoa _search = await db.TabelaPessoas.Where(x => x.ID == pessoa.ID).FirstOrDefaultAsync();
            _search.ProdutoID = pessoa.ProdutoID;
            _search.CPF = pessoa.CPF.Replace(".", "").Replace("-", "");
            _search.Nome = pessoa.Nome;
            _search.Email = pessoa.Email;

            db.Entry(_search).State = EntityState.Modified;
            await db.SaveChangesAsync();

            TempData["MENSAGEM"] = new MensagemAviso { Tipo = 1, Descricao = "EDIÇÃO REALIZADA COM SUCESSO!" };
            return RedirectToAction("Index");
        }

        // GET: Pessoas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pessoa pessoa = await db.TabelaPessoas.FindAsync(id);
            if (pessoa == null)
            {
                return HttpNotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pessoa pessoa = await db.TabelaPessoas.FindAsync(id);
            db.TabelaPessoas.Remove(pessoa);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CpfCheck(string CPF, int? ID)
        {
            string _CpfSemMascara = CPF.Replace(".", "").Replace("-", "");
            bool _result = db.TabelaPessoas.Where(x => x.CPF == _CpfSemMascara && x.ID != ID && x.Deletado == false).Count() > 0 ? false : true;
            return _result;
        }

        private bool EmailCheck(string Email, int? ID)
        {
            bool _result = db.TabelaPessoas.Where(x => x.Email == Email && x.ID != ID && x.Deletado == false).Count() > 0 ? false : true;
            return _result;
        }

        private bool ProdutoCheck(int ProdutoID, int? ID)
        {
            Produto _associacao = db.TabelaProdutos
                .Where(x => x.Deletado == false && x.ID == ProdutoID)
                .Include(x => x.Pessoas).FirstOrDefault();
            if (_associacao.Pessoas.Where(x => x.Deletado == false && x.ID != ID && x.Deletado == false).Count() > 0)
            {
                return false;
            }
            return true;
        }

        //---------------------------------------------------------------------------------------
        // CHAMADAS REMOTAS DE VERIFICAÇÃO
        //---------------------------------------------------------------------------------------
        public JsonResult RemoteCPF(string CPF, int? ID)
        {
            return Json(CpfCheck(CPF, ID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoteEmail(string Email, int? ID)
        {
            return Json(EmailCheck(Email, ID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoteProduto(int ProdutoID, int? ID)
        {
            return Json(ProdutoCheck(ProdutoID, ID), JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
