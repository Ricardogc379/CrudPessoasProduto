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
    public class ProdutosController : Controller
    {
        private TimiproDB db = new TimiproDB();

        // GET: Produtos
        public async Task<ActionResult> Index()
        {
            return View(await db.TabelaProdutos.Where(x => x.Deletado == false).ToListAsync());
        }

        // GET: Produtos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = await db.TabelaProdutos.FindAsync(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Produtos/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Produto produto)
        {
            db.TabelaProdutos.Add(produto);
            await db.SaveChangesAsync();
            TempData["MENSAGEM"] = new MensagemAviso
            {
                Tipo = 1,
                Descricao = "PRODUTO " + produto.Nome + " CADASTRADO COM SUCESSO!"
            };
            return RedirectToAction("Index");
        }

        // GET: Produtos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = await db.TabelaProdutos.FindAsync(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Produto produto)
        {
            Produto _search = await db.TabelaProdutos.Where(x => x.ID == produto.ID).FirstOrDefaultAsync();
            if (produto == null)
            {
                return RedirectToAction("Index");
            }
            //----------------------------------------------------------------------------------------
            // ALTERA OS CAMPOS DO PRODUTO
            //----------------------------------------------------------------------------------------
            _search.ChangeProperties<Produto>(produto);
            //----------------------------------------------------------------------------------------
            db.Entry(_search).State = EntityState.Modified;
            await db.SaveChangesAsync();
            TempData["MENSAGEM"] = new MensagemAviso
            {
                Tipo = 1,
                Descricao = "PRODUTO " + produto.Nome + " EDITADO COM SUCESSO!"
            };
            return RedirectToAction("Index");
        }

        // GET: Produtos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Produto produto = await db.TabelaProdutos.Where(x => x.ID == id).FirstOrDefaultAsync();
            //----------------------------------------------------------------------------------------
            // VERIFICA SE O PRODUTO ESTÁ ASSOCIADO A UMA PESSOA
            //----------------------------------------------------------------------------------------
            if (produto != null && produto.Pessoas.Where(x => x.Deletado == false).Count() > 0)
            {
                TempData["MENSAGEM"] = new MensagemAviso
                {
                    Tipo = 2,
                    Descricao = "PRODUTO " + produto.Nome + " ESTÁ ASSOCIADO A UMA PESSOA!"
                };
                return RedirectToAction("Index");
            }
            //----------------------------------------------------------------------------------------
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: Produtos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Produto produto = await db.TabelaProdutos.Where(x => x.ID == id).FirstOrDefaultAsync();
            //----------------------------------------------------------------------------------------
            // VERIFICA SE O PRODUTO ESTÁ ASSOCIADO A UMA PESSOA
            //----------------------------------------------------------------------------------------
            if (produto != null && produto.Pessoas.Where(x => x.Deletado == false).Count() > 0)
            {
                TempData["MENSAGEM"] = new MensagemAviso
                {
                    Tipo = 2,
                    Descricao = "PRODUTO " + produto.Nome + " ESTÁ ASSOCIADO A UMA PESSOA!"
                };
                return RedirectToAction("Index");
            }
            //----------------------------------------------------------------------------------------
            db.TabelaProdutos.Remove(produto);
            await db.SaveChangesAsync();
            TempData["MENSAGEM"] = new MensagemAviso
            {
                Tipo = 1,
                Descricao = "PRODUTO " + produto.Nome + " EXCLUIDO COM SUCESSO!"
            };
            return RedirectToAction("Index");
        }

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
