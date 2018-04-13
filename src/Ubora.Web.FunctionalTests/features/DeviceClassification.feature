Feature: Device classification
    As a project leader
    I want to go trough device classification questionnaire

Background:
    Given I am signed in as user and on first page
        And I click on the element "h5=Test title"
        And I click on the element "a=Work packages"
        And I click on the element "*=Medical need and product specification" inside "main"
        And I click on the element "*=Device classification" inside "main"

Scenario: I go through Device classification and pick always first option
    Then I expect the title of the page "Device classification - UBORA"
    When I click on the element "button=Start classifying your device"
		And I answer "Non-invasive" to the question "Is your device INVASIVE or NON-INVASIVE?"
        And I answer "Yes" to the question "Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?"
        And I answer "Yes" to the question "May it be connected to an active medical device in class IIa or a higher class?"
        And I answer "Yes" to the question "Is it intended for use for storing or channelling blood or other body liquids or for storing organs, parts of organs or body cells and tissues, but it is not a blood bag?"
        And I answer "Yes" to the question "Is it a blood bag?"
        And I answer "Yes" to the question "Is it intended for modifying the biological or chemical composition of human tissues or cells, blood, other body liquids or other liquids intended for implantation or administration into the body?"
        And I answer "Yes" to the question "Does the treatment consist of filtration, centrifugation or exchanges of gas, heat?"
        And I answer "Yes" to the question "Is the treatment other than filtration, centrifugation or exchanges of gas, heat?"
        And I answer "Yes" to the question "Is your device a substance or a mixture of substances intended to be used in vitro in direct contact with human cells, tissues or organs taken off from the human body or with human embryos before their implantation or administration into the body?"
        And I answer "Yes" to the question "Is it intended come into contact with injured skin or mucous membrane?"
        And I answer "Yes" to the question "Is it intended to be used as a mechanical barrier, for compression or for absorption of exudates?"
        And I answer "Yes" to the question "Is it intended to be used principally for injuries to skin which have breached the dermis or mucous membrane and can only heal by secondary intent?"
        And I answer "Yes" to the question "Is it principally intended to manage the micro-environment of injured skin or mucous membrane?"
        And I answer "Yes" to the question "Is your device ACTIVE?"
        And I answer "Yes" to the question "Is it therapeutic?"
        And I answer "Yes, in a potentially hazardous way (consider nature of energy, density of energy, site of the body)." to the question "Does it administer energy to or exchange energy with the human body?"
        And I answer "Yes" to the question "Is it intended to emit ionizing radiation?"
        And I answer "Yes" to the question "Is it intended for therapeutic radiology, including interventional radiology devices and devices which control or monitor such devices, or which directly influence their performance?"
        And I answer "Yes" to the question "Is it intended to control or monitor a device that emits ionizing radiation, or does it directly influence the performance a device that emits ionizing radiation?"
        And I answer "Yes" to the question "Is it intended to control or monitor the performance of active therapeutic class IIb devices, or intended directly to influence the performance of such devices?"
        And I answer "Yes" to the question "Is it intended for controlling, monitoring or directly influencing the performance of active implantable devices?"
        And I answer "Yes" to the question "Is it intended for diagnosis and monitoring?"
        And I answer "Yes" to the question "Is it intended to supply energy which will be absorbed by the human body?"
        # the question has been cut short because of WebdriverIO anomaly
        And I answer "Yes" to the question "illuminate" 
        And I answer "Yes" to the question "Does it supply energy different than light in the visible spectrum?"
        And I answer "Yes" to the question "Is it intended to image in vivo distribution of radiopharmaceuticals?"
        And I answer "Yes" to the question "Is it intended to allow direct diagnosis or monitoring of vital physiological processes?"
        And I answer "Yes" to the question "Are the vital physiological processes such as the patient may be in immediate danger if the vary (example: variations in cardiac performance, respiration, activity of the central nervous system)?"
        And I answer "Yes, and diagnosis and/or monitoring are done in a clinical situation while the patient is in immediate danger." to the question "Is the device intended for diagnosis and monitoring?"
        And I answer "Yes" to the question "Is it intended to emit ionizing radiation for diagnostics?"
        And I answer "Yes" to the question "Is it a software?"
        And I answer "Yes" to the question "Is it intended to provide information which is used to take decisions with diagnosis or therapeutic purposes?"
        And I answer "Yes, an impact that may cause death or an irreversible deterioration of a person's state of health." to the question "Does such a device have a significant impact?"
        And I answer "Yes" to the question "Is it intended to monitor physiological processes?"
        And I answer "Yes, it monitors vital physiological parameters, where the nature of variations of those parameters is such that it could result in immediate danger to the patient." to the question "Does it monitor parameters?"
        And I answer "Yes" to the question "Is it intended to administer and/or remove medicinal products, body liquids or other substances to or from the body?"
        And I answer "Yes" to the question "Is this done in a manner that is potentially hazardous (taking account of the nature of the substances involved, of the part of the body concerned and of the mode of application)?"
        And I answer "Yes" to the question "Does your device incorporate a medicinal product with an ancillary function?"
        And I answer "Yes" to the question "Is it used for contraception or prevention of the transmission of sexually transmitted diseases?"
        And I answer "Yes" to the question "Is it implantable or long term invasive?"
        And I answer "Yes" to the question "Is it intended specifically to be used for disinfecting, cleaning, rinsing or, where appropriate, hydrating contact lenses?"
        And I answer "Yes" to the question "Is it intended specifically to be used for disinfecting or sterilising medical devices that are not contact lenses ? NOTE: If it is only intended for cleaning and only by physical action, this rule does not apply."
        And I answer "Yes" to the question "Is it a disinfecting solution or washer-disinfector, intended specifically to be used for disinfecting invasive devices, as the end point of processing?"
        And I answer "Yes" to the question "Is it specifically intended for recording of diagnostic images generated by X-ray radiation?"
        And I answer "Yes" to the question "Is it manufactured utilizing tissues or cells of human or animal origin, or their derivatives, which are non-viable or rendered non-viable?"
        And I answer "Yes" to the question "Does it incorporate or contain nanomaterials?"
        And I answer "High or medium" to the question "What is the potential for internal exposure?"
        And I answer "Yes" to the question "Is it intended to administer medicinal products by inhalation, being an invasive device with respect to body orifices, other than surgically invasive devices?"
        And I answer "Yes" to the question "Does its mode of action have an essential impact on the efficacy and safety of the administered medicinal product?"
        And I answer "Yes" to the question "Is it intended to treat life-threatening conditions?"
        And I answer "Yes" to the question "Is it composed of substances or of combinations of substances that are intended to be introduced into the human body via a body orifice or applied to the skin and that are absorbed by or locally dispersed in the human body?"
        And I answer "Yes" to the question "Is it or it products of metabolism, systemically absorbed by the human body in order to achieve the intended purpose?"
        And I answer "Yes" to the question "Does it achieve its intended purpose in the stomach or lower gastrointestinal tract and it, or its products of metabolism, is systemically absorbed by the human body?"
        And I answer "Yes" to the question "Is it applied to the skin or is it applied in the nasal or oral cavity as far as the pharynx, and achieve the intended purpose on those cavities?"
        And I answer "Yes" to the question "Is it an active therapeutic device with an integrated or incorporated diagnostic function that includes an integrated or incorporated diagnostic function which significantly determines the patient management by the device (example: closed loop systems or automated external defibrillators)?"
    Then I expect the element "h1=Chosen class: III" is visible
        And I expect the title of the page "Device classification - UBORA"
    When I click on the element "a=Overview"
    Then I expect the element "dd=III" is visible

Scenario: I go through Device classification and always answer NO
    Then I expect the element "p=Results:" is visible
    When I click on the element "button=Start classifying your device"
        And I answer "Non-invasive" to the question "Is your device INVASIVE or NON-INVASIVE?"
        And I answer "No" to the question "Is it intended for channelling or storing blood, body liquids, cells or tissues, liquids or gases for the purpose of eventual infusion, administration or introduction into the body?"
        And I answer "No" to the question "Is your device ACTIVE?"
        And I answer "No" to the question "Does your device incorporate a medicinal product with an ancillary function?"
        And I answer "No" to the question "Is it used for contraception or prevention of the transmission of sexually transmitted diseases?"
        And I answer "No" to the question "Is it intended specifically to be used for disinfecting, cleaning, rinsing or, where appropriate, hydrating contact lenses?"
        And I answer "No" to the question "Is it intended specifically to be used for disinfecting or sterilising medical devices that are not contact lenses ? NOTE: If it is only intended for cleaning and only by physical action, this rule does not apply."
        And I answer "No" to the question "Is it manufactured utilizing tissues or cells of human or animal origin, or their derivatives, which are non-viable or rendered non-viable?"
        And I answer "No" to the question "Does it incorporate or contain nanomaterials?"
        And I answer "No" to the question "Is it intended to administer medicinal products by inhalation, being an invasive device with respect to body orifices, other than surgically invasive devices?"
        And I answer "No" to the question "Is it composed of substances or of combinations of substances that are intended to be introduced into the human body via a body orifice or applied to the skin and that are absorbed by or locally dispersed in the human body?"
        And I answer "No" to the question "Is it an active therapeutic device with an integrated or incorporated diagnostic function that includes an integrated or incorporated diagnostic function which significantly determines the patient management by the device (example: closed loop systems or automated external defibrillators)?"
    Then I expect the element "h1=Chosen class: I" is visible
    When I click on the element "a=Overview"
    Then I expect the element "dd=I" is visible