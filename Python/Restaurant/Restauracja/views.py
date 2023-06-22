from django.shortcuts import render, redirect
from django.http import HttpResponse
from .models import Product
from .models import Mails
from .forms import MenuForm

from carton.cart import Cart

def index(request):
    return render(request, 'Restauracja/index.html')

def menu(request):
    form = MenuForm()
    if request.method == 'GET':
        products = Product.objects.all()
    if request.method == 'POST':
        form = MenuForm(request.POST)
        if form.is_valid():
            menu_type = form.cleaned_data['menu_type']
            products = Product.objects.filter(product_type=menu_type)
        else:
            form = MenuForm()
            products = Product.objects.all()
    return render(request, 'Restauracja/menu.html', {'products': products, 'form': form})

def contact(request):
    email = "Wprowadź adres e-mail"
    success = False
    if request.method == 'POST':
        address = request.POST.get('email')
        email = Mails(mail = address)
        email.save()
        email = "Twój adres E-mail " + str(email)
        success = True
    return render(request, 'Restauracja/contact.html', {'success' : success, 'mail': email})

def orders(request):
    products = Product.objects.all()
    cart = Cart(request.session)

    if request.method == 'POST':
        product_id = request.POST.get('product_id_add')
        if product_id:
            product = Product.objects.get(id=product_id)
            cart.add(product, price=product.price)
            
        product_remove_id = request.POST.get('product_id_remove')
        if product_remove_id:
            product = Product.objects.get(id=product_remove_id)
            cart.remove_single(product)
        
        clear = request.POST.get('clear')
        if clear:
            cart.clear()
    return render(request, 'Restauracja/orders.html', {'products': products})

def about(request):
    return render(request, 'Restauracja/about.html')
