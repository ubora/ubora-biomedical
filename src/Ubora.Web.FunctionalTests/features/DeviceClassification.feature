Feature: Device classification
    As a project leader
    I want to go trough device classification questionnaire

Background:
    Given I am signed in as user and on first page
        And I click on the element "h4=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "span=Medical need and product specification"
        And I click on the element "span=Device classification"

Scenario: I go through Device classification
    Then I expect the title of the page "Device classification - UBORA"
    When I click on the element "button=Take questionnaire"
    When I answer "Non-invasive" to the question "Is your device INVASIVE or NON-INVASIVE?"
        And I answer "Yes" to the question "Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?"
        And I answer "Yes" to the question "May it be connected to an active medical device in class IIa or a higher class?"
        And I answer "Yes" to the question "Is it intended for use for storing or channelling blood or other body liquids or for storing organs, parts of organs or body cells and tissues, but it is not a blood bag?"
        And I answer "Yes" to the question "Is it a blood bag?"
        And I click on the element "button=Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?"
        And I click on the element "button=May it be connected to an active medical device in class IIa or a higher class?"
        And I click on the element "button=Is your device ACTIVE?"
        And I click on the element "button=Is it therapeutic?"
        And I click on the element "button=Does it administer energy to or exchange energy with the human body in a potentially hazardous way (consider nature of energy, density of energy, site of the body)?"
        And I click on the element "button=Does your device incorporate a MEDICAL PRODUCT with an ancillary function?"
        And I click on the element "button=Is it used for contraception or prevention of the transmission of sexually transmitted diseases?"
        And I click on the element "button=Is it implantable or long term invasive?"
        And I click on the element "button=Is it intended specifically to be used for disinfecting, cleaning, rinsing or, where appropriate, hydrating contact lenses?"
        And I click on the element "button=Is it intended specifically to be used for disinfecting or sterilising medical devices that are not contact lenses?"
        And I click on the element "button=Is it a disinfecting solution or washer-disinfector, intended specifically to be used for disinfecting invasive devices, as the end point of processing?"
        And I click on the element "button=Is it manufactured utilising tissues or cells of human or animal origin, or their derivatives, which are non-viable or rendered non-viable?"
        And I click on the element "button=Does it incorporate or contain nanomaterials?"
        And I click on the element "button=Does it present a high or medium potential for internal exposure?"
        And I click on the element "button=Is it intended to administer medicinal products by inhalation, being an invasive device with respect to body orifices, other than surgically invasive devices?"
        And I click on the element "button=Does its mode of action have an essential impact on the efficacy and safety of the administered medicinal product?"
        And I click on the element "button=Is it composed of substances or of combinations of substances that are intended to be introduced into the human body via a body orifice or applied to the skin and that are absorbed by or locally dispersed in the human body?"
        And I click on the element "button=Is it or it products of metabolism, systemically absorbed by the human body in order to achieve the intended purpose?"
        And I click on the element "button=Is it an active therapeutic devices with an integrated or incorporated diagnostic function that includes an integrated or incorporated diagnostic function which significantly determines the patient management by the device (example: closed loop systems or automated external defibrillators)?"
    Then I expect the element "h1=Your device classification is: III" is visible
        And I expect the title of the page "Device classification results - UBORA"
    When I click on the element "a=Retake questionnaire?"
    Then I expect the title of the page "Device classification - UBORA"
    When I go back to last page
    Then I expect the title of the page "Device classification results - UBORA"
    When I click on the element "a=Go back to project page"
    Then I expect the title of the page "Dashboard - UBORA"