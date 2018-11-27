using MartinKMe.Interfaces;
using MartinKMe.Models.ArticlesViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MartinKMe.Controllers
{
    public class ArticlesController : Controller
    {
        private const int _itemsPerPage = 10;
        private readonly IStore _store;

        public ArticlesController(IStore store)
        {
            _store = store;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            // all published articles
            var articles = await _store.GetContents();

            // get page
            var pageOfArticles = articles
                .Skip((page-1) * _itemsPerPage)
                .Take(_itemsPerPage)
                .ToList();

            // calculate number of pages
            var pagesCount = (double)articles.Count / (double)_itemsPerPage;
            int pageCountRounded = Convert.ToInt16(Math.Ceiling(pagesCount));

            var vm = new IndexViewModel()
            {
                Articles = pageOfArticles,
                ItemsPerPage = _itemsPerPage,
                Pages = pageCountRounded,
                ThisPage = page,
                TotalItems = articles.Count
            };

            return View(vm);
        }

        public async Task<IActionResult> Article(string article)
        {
            var thisItem = await _store.GetContent(article);

            var vm = new ArticleViewModel()
            {
                Article = thisItem,
                Html = new HtmlString(thisItem.Html)
            };

            return View(vm);
        }
    }
}