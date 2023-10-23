﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ubique.DataAccess.Repository.IRepository;
using Ubique.Models;
using Ubique.Models.ViewModels;

namespace Ubique.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Product> productList = _unitOfWork.Product.GetAll().ToList();

			foreach (Product product in productList)
			{
				SubCategory subCategory = _unitOfWork.SubCategory.Get(u => u.Id == product.SubCategoryId);
				subCategory.Category = _unitOfWork.Category.Get(u => u.Id == subCategory.CategoryId);
				product.SubCategory = subCategory;
			}

			return View(productList);
		}

		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new ProductVM();
			productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

			if (id != 0 && id != null)
			{
				// update
				productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
				productVM.Product.SubCategory = _unitOfWork.SubCategory.Get(u => u.Id == productVM.Product.SubCategoryId);

				IEnumerable<SubCategory>? subCategories = _unitOfWork.SubCategory.GetList(u => u.CategoryId == productVM.Product.SubCategory.CategoryId);

				if (subCategories != null)
				{
					IEnumerable<SelectListItem> subCategoriesItems =
						subCategories.Select(subCategory => new SelectListItem()
						{
							Text = subCategory.Name,
							Value = subCategory.Id.ToString()
						});

					productVM.SubCategoryList = subCategoriesItems;
				}
			}
			else
			{
				// insert
				productVM.Product = new Product();
				productVM.Product.SubCategory = new SubCategory();
				productVM.Product.SubCategory.Category = new Category();
			}

			return View(productVM);
		}

		[HttpPost]
		public IActionResult Upsert(ProductVM productVM, IFormFile? file)
		{
			bool newProductHasImage = productVM.Product.Id == 0 && file != null;

			if (productVM.Product.IsValid() && newProductHasImage)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;

				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, @"images\product");

					if (productVM.Product.Id != 0)
					{
						string oldImagePath = Path.Combine(wwwRootPath, _unitOfWork.Product.Get(u => u.Id == productVM.Product.Id).ImageUrl.TrimStart('\\'));

						if (!string.IsNullOrEmpty(oldImagePath) && System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

					productVM.Product.ImageUrl = @"\images\product\" + fileName;
				}

				if (productVM.Product.Id == 0)
				{
					productVM.Product.SubCategory = _unitOfWork.SubCategory.Get(u => u.Id == productVM.Product.SubCategoryId);
					productVM.Product.SubCategory.Category = _unitOfWork.Category.Get(u => u.Id == productVM.Product.SubCategory.CategoryId);
				}

				if (productVM.Product.Id == 0)
				{
					_unitOfWork.Product.Add(productVM.Product);
				}
				else
				{
					_unitOfWork.Product.Update(productVM.Product);
				}

				_unitOfWork.Save();
				TempData["success"] = "Prodotto creato con successo!";

				return RedirectToAction("Index");
			}
			else
			{
				productVM.CategoryList = _unitOfWork.Category
					.GetAll()
					.ToList()
					.Select(u => new SelectListItem
					{
						Text = u.Name,
						Value = u.Id.ToString()
					});

				productVM.SubCategoryList = _unitOfWork.SubCategory
					.GetList(u => u.CategoryId == productVM.Product.SubCategoryId)
					.ToList()
					.Select(u => new SelectListItem
					{
						Text = u.Name,
						Value = u.Id.ToString()
					});

				return View(productVM);
			}
		}

		[HttpGet]
		public IActionResult GetSubCategoriesBasedOnCategory(int id)
		{
			IEnumerable<SubCategory> subCategories = _unitOfWork.SubCategory.GetList(u => u.CategoryId == id);
			IEnumerable<SelectListItem> listItems = subCategories.Select(subCategory => new SelectListItem
			{
				Text = subCategory.Name,
				Value = subCategory.Id.ToString()
			});

			return new JsonResult(listItems);
		}

		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

			if (productFromDb == null)
			{
				return NotFound();
			}

			return View(productFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Product obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Prodotto aggiornato con successo!";
				return RedirectToAction("Index");
			}

			return View();
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

			if (productFromDb == null)
			{
				return NotFound();
			}

			return View(productFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePOST(int? id)
		{
			Product? obj = _unitOfWork.Product.Get(u => u.Id == id);

			if (obj == null)
			{
				return NotFound();
			}

			_unitOfWork.Product.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Prodotto rimosso con successo!";
			return RedirectToAction("Index");
		}
	}
}