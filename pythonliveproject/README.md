# Python Django Live Project

## Introduction

This was a two week sprint working in Pycharm with Python and the Django Framework.
This project gave me my first opportunity to use Azure DevOps along with using Git
to create my first working branches, perform commits, local merges as pushing them
back for review to be merged by the project manager or be given back to me for revisions.

I was tasked with creating an interactive website for managing a collection of user 
data relating to various hobbies. I decided to make mine craft beer(someone else on
the team already had hiking trails). This app features full CRUD funtionality for adding,
viewing, editing and deleting breweries as well as the ability to sort and column
alphabetically in either direction.

## Building the App

I created the App through Django and registered it within the main project. I then 
created my base and home templates and added the fucntion tot render those pages.
Last, I created the URLs file and registered that within the main app as well.

## Creating the Model and Form

I created two models for this project, one is a brewery object and the other is an 
object representing all breweries. I also created a choices list for the beer styles at
each brewery.

```python
# brewery models, only second beerstyle and notes can be empty
class Brewery(models.Model):
    name = models.CharField(max_length=50, default='', blank=False, null=False)
    address1 = models.CharField(max_length=50, default='', blank=False, null=False)
    address2 = models.CharField(max_length=50, default='', blank=True, null=True)
    city = models.CharField(max_length=50, default='', blank=False, null=False)
    state = models.CharField(max_length=50, default='Select State', choices=STATE_CHOICES, blank=False, null=False)
    zip = models.IntegerField(blank=False, null=False)
    beerstyles1 = models.CharField(max_length=50, default='', choices=BEER_CHOICES, blank=False, null=False)
    beerstyles2 = models.CharField(max_length=50, default='', choices=BEER_CHOICES, blank=True, null=True)
    notes = models.TextField(max_length=250, default='', blank=True, null=True)
    objects = models.Manager()

    def __str__(self):
        return self.name

class AllBreweries(models.Model):
    allbreweries = Brewery.objects.all()
```
```python
# state choices and beer specialty choices
BEER_CHOICES = [('IPAS', 'IPAs'),
                ('SOURS', 'Sours'),
                ('BARREL-AGED', 'Barrel-Aged'),
                ('DESSERT', 'Dessert/Fruited')
                ]
```
To create the user input form, I created a new forms.py and utilized a few 
Django widgets to display choices on the users end. I made a template page for 
the form, then I created a views function to render this page and connect it to 
my URLS file. This project used the Django package CrispyForms.

```python
class BreweryForm(ModelForm):
    class Meta:
        model = Brewery
        fields = '__all__'
        widgets = {
            'notes': forms.Textarea(),
        }

    def __init__(self, *args, **kwargs):
        super().__init__(*args, **kwargs)
        self.helper = FormHelper()
        self.fields['name'].label = "Brewery Name:"
        self.fields['address1'].label = "Address Line 1:"
        self.fields['address2'].label = "Address Line 2:"
        self.fields['beerstyles1'].label = "Best Style:"
        self.fields['beerstyles2'].label = "Second Best:"
        self.fields['notes'].label = "Field Notes:"
        self.helper.layout = Layout(
            Row(
                Column('name', css_class='form-group form-control-md makeinline col-md-6 mb-1'),
                Column('beerstyles1', css_class='form-group makeinline col-md-3 mb-1'),
                Column('beerstyles2', css_class='form-group makeinline col-md-3 mb-1'),
                css_class='form-row'
            ),
            Row(
                Column('address1', css_class='form-group makeinline col-md-6 mb-1'),
                Column('address2', css_class='form-group makeinline col-md-6 mb-1'),
                css_class='form-row'
            ),
            Row(
                Column('city', css_class='form-group makeinline col-md-6 mb-1'),
                Column('state', css_class='form-group makeinline col-md-3 mb-1'),
                Column('zip', css_class='form-group makeinline col-md-3 mb-1'),
                css_class='form-row'
            ),
            Row(
                Column('notes', css_class='form-group col-md-12 mb-1'),
            ),
            Row(
                Submit('submit', 'Save', css_class='btn btn-primary col-2 ml-auto mb-3'),
                HTML('<a class="btn btn-dark col-2 ml-1 mr-3 mb-3" href="">Back</a>'),
            ),
        )
```

## CRUD Funtionality

After creating the model it was time to add the ability to Create, Read, Update(Edit) and Delete. 
To start we add corresponding functions.

## Create

```python
# create and save new brewery
def BA_addnew(request):
    form = BreweryForm(request.POST or None)
    if form.is_valid():
        form.save()
        return redirect('BA_addnew')
    else:
        print(form.errors)
        form = BreweryForm()
    context = {
        'form': form,
    }
    return render(request, 'BreweriesApp/BreweriesApp_addnew.html', context)
```
## Read

```python
# render all breweries, change sorting between asc/desc by clicking table headers
def BA_index(request):
    orderby = request.GET.get('order_by', 'name')
    sortby = request.GET.get('sort', 'ascending')
    if sortby == 'descending':
        allbreweries = Brewery.objects.all().order_by('-' + orderby)#appends and saves new state to context
        sortby = "ascending" #change the url to descedning
    elif sortby == "ascending":
        allbreweries = Brewery.objects.all().order_by(orderby)#appends and saves new state to context
        sortby = "descending" #changes the url to descening
    context = { 'allbreweries': allbreweries, 'sortby': sortby}
    return render(request, 'BreweriesApp/BreweriesApp_index.html', context)
```

## Details View

```python
# render details page
def BA_details(request, _id):
    try:
        details = Brewery.objects.get(id=_id)
    except Brewery.DoesNotExist:
        raise get_object_or_404('Brewery does not exist!')

    return render(request, 'BreweriesApp/BreweriesApp_details.html', context={'details': details})
```
## Update
```python
# render edit page pulling data for given brewery
def BA_edit(request, _id):
    form = get_object_or_404(Brewery, id=_id)
    if request.method == 'POST':
        form = BreweryForm(request.POST, instance=form)
        if form.is_valid():
            form = form.save()
            form.save()
            return redirect('../../../all/')
    else:
        form = BreweryForm(instance=form)

    return render(request, 'BreweriesApp/BreweriesApp_edit.html', context={'form': form})
```

## Delete

```python
# render delete page and pull the id to be deleted
def BA_delete(request, _id):
    context = {}
    entry = get_object_or_404(Brewery, id=_id)
    if request.method == "POST":
        entry.delete()
        return HttpResponseRedirect('../../../all/')
    return render(request, 'BreweriesApp/BreweriesApp_index.html', context)
```

Pulling data for the breweries for the index view with alphabetical sorting options by column.
```html
<div class="table-container">
    <table class="table table-hover">
        <tr>
            <th class="thead"><a href="?order_by=name&sort={{sortby}}">Name</a></th>
            <th class="thead"><a href="?order_by=beerstyles1&sort={{sortby}}">Style 1</a></th>
            <th class="thead"><a href="?order_by=beerstyles2&sort={{sortby}}">Style 2</a></th>
            <th class="thead"><a href="?order_by=city&sort={{sortby}}">City</a></th>
            <th class="thead"><a href="?order_by=state&sort={{sortby}}">State</a></th>
        </tr>
        {% for b in allbreweries %}
        <tr>
            <td scope="col-md-4"><a href="{% url 'BA_details' b.id %}">{{ b.name }}</a></td>

            <td scope="col-md-2">{{ b.beerstyles1 }}</td>

            <td scope="col-md-2">{{ b.beerstyles2 }}</td>

            <td scope="col-md-2">{{ b.city }}</td>

            <td scope="col-md-2">{{ b.state }}</td>
        </tr>
        {% endfor %}
    </table>
    <br><br><br>
</div>
```
