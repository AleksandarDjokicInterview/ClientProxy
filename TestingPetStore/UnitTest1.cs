using NUnit.Framework;
using PetStoreClientProxy;
using System;
using System.Collections;
using System.Linq;

namespace TestingPetStore
{
    public class Tests
    {
        //Arange
        Client _client;
        Pet _pet;

        [SetUp]
        public void Setup()
        {
            _client = new Client();
            _pet = new Pet
            {
                Id = 44,
                Category = new Category
                {
                    Id = 190,
                    Name = "test"
                },
                PhotoUrls = new string[]
                {
                    "https://cdn.pixabay.com/photo/2017/09/25/13/12/dog-2785074_960_720.jpg"
                },
                Tags = new Tag[]
                {
                    new Tag() {
                        Id = 33,
                        Name = "black"
                    },
                    new Tag()
                    {
                        Id = 34,
                        Name = "sad"
                    }
                },
                Name = "TestPet1",
                Status = PetStatus.Available,
                
            };
        }

        [Test(Author ="Aleksandar Djokic",Description ="Testing Creation and retrieval of Pet object")]
        [Category("Create pet")]
        public void AddPetAsync_ValidDataInput_ExpectAssertionToPass()
        {
            //Act
            _client.AddPetAsync(_pet).GetAwaiter().GetResult();

            //Assert
            var actual = _client.GetPetByIdAsync(_pet.Id.Value).GetAwaiter().GetResult();
            AssertCreatedPet(actual);
        }
        [Test(Author = "Aleksandar Djokic", Description = "Testing Updating and retrieval of updated Pet object")]
        [Category("Update pet")]
        public void UpdatePetAsync_ValidDataInput_ExpectAssertionToPass()
        {
            _client.AddPetAsync(_pet).GetAwaiter().GetResult();
            _pet.Name = "TestPetNameUpdated";
            //Act
            _client.UpdatePetAsync(_pet).GetAwaiter().GetResult();

            //Assert
            var actual = _client.GetPetByIdAsync(_pet.Id.Value).GetAwaiter().GetResult();
            AssertCreatedPet(actual);
        }

        [Test(Author = "Aleksandar Djokic", Description = "Deleting Pet object")]
        [Category("Delete pet")]
        public void DeletePetAsync_ValidDataInput_ExpectDataToBeDeleted()
        {
            _client.AddPetAsync(_pet).GetAwaiter().GetResult();
            var actual = _client.GetPetByIdAsync(_pet.Id.Value).GetAwaiter().GetResult();
            AssertCreatedPet(actual);
            //Act
            _client.DeletePetAsync("", _pet.Id.Value).GetAwaiter().GetResult();

            //Assert
            Assert.Throws(typeof(ApiException), () => _client.GetPetByIdAsync(_pet.Id.Value).GetAwaiter().GetResult());
            
        }
        private void AssertCreatedPet(Pet actual)
        {
            Assert.AreEqual(actual.Name, _pet.Name);
            Assert.AreEqual(actual.PhotoUrls.First(), _pet.PhotoUrls.First());
            Assert.AreEqual(actual.Status, _pet.Status);
            CollectionAssert.IsNotEmpty(actual.Tags);
            Assert.IsNotNull(actual.Tags.First(d => d.Id == _pet.Tags.Last().Id && d.Name.Equals(_pet.Tags.Last().Name)));
            Assert.IsNotNull(actual.Tags.First(d => d.Id == _pet.Tags.First().Id && d.Name.Equals(_pet.Tags.First().Name)));
            Assert.AreEqual(actual.Category.Id, _pet.Category.Id);
            Assert.AreEqual(actual.Category.Name, _pet.Category.Name);
        }
    }
  

}