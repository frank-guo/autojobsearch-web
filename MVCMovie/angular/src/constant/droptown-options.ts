export const cities = {
    'BC': ['Vancouver', 'Burnaby', 'Richmond', 'Coquitlam', 'Surrey', 'Port Coquitlam'],
    'ON': ['Toronto', 'Ajax', 'Clarington', 'Mississauga', 'Richmond Hill']
};

export const provinces = [{ id: 'AB', text: 'Albert' }, { id: 'BC', text: 'British Columbia' }, { id: 'QC', text: 'Quebec' }, {id: 'ON', text: 'Ontario'}]

export const titles = ['Software Engineer', 'Software Developer', 'Web Developer', 'Payroll Specialist']

export function allCities() {
    let allcities: string[] = []
    Object.keys(cities).map((key) => {
        allcities = allcities.concat(cities[key])
    })

    return allcities
}