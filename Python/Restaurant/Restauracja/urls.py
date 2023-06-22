from django.urls import path
from django.conf import settings
from django.conf.urls.static import static
from . import views
        
urlpatterns = [
    path("", views.index, name="index"),
    path("menu/", views.menu, name="menu"),
    path("contact/", views.contact, name="contact"),
    path("orders/", views.orders, name="orders"),
    path("about/", views.about, name="about"),
]

if settings.DEBUG:
        urlpatterns += static(settings.MEDIA_URL,
                              document_root=settings.MEDIA_ROOT)
