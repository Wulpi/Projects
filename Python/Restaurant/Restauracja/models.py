from django.db import models

# Create your models here.

class Product(models.Model):
    PRODUCT_TYPES = (
        ('pizza', 'Pizza'),
        ('dania_glowne', 'Dania Główne'),
        ('napoje', 'Napoje'),
        ('przystawki', 'Przystawki')
    )
    product_name = models.CharField(max_length=50)
    decription = models.CharField(max_length=150)
    price = models.DecimalField(decimal_places=2, max_digits=6)
    product_type = models.CharField(max_length=15, choices=PRODUCT_TYPES)
    image = models.ImageField(upload_to='product_images/')
    def __str__(self):
        return self.product_name
    
class Mails(models.Model):
    mail = models.CharField(max_length=100)
    def __str__(self):
        return self.mail


