# Generated by Django 4.2 on 2023-06-02 17:19

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('Restauracja', '0004_mails_alter_product_product_type'),
    ]

    operations = [
        migrations.AddField(
            model_name='product',
            name='image',
            field=models.ImageField(default='default', upload_to='product_images/'),
            preserve_default=False,
        ),
    ]
