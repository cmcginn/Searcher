var ViewModel = function () {
    
    function addTerm(e) {
        vm.searchTerms.push({ term: '', score: 0 });
    };
    function removeTerm(e) {
        vm.searchTerms.remove(e);
    };
    function save() {
        $('form').parsley().destroy();
        $('form').parsley();
        var valid = $('form').parsley().validate();
        if (valid) {
            
        };
    };
    var model = {
        searchName:null,
        searchTerms: [],
        addTerm: addTerm,
        removeTerm: removeTerm,
        save:save
    };
   
    var vm = ko.mapping.fromJS(model);

    return vm;
 
}
var viewModel = new ViewModel();
ko.applyBindings(viewModel);